using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using TeamsHook.NET;
using Xml2SqlImport.Data.SPModels;
using Xml2SqlImport.Helpers.Responses;
using Xml2SqlImport.Interfaces;

namespace Xml2SqlImport.Services
{
    public class TeamsService : ITeamsService
    {
        private readonly ILocalDataService _db;
        private readonly IAppSettings _appSettings;

        public TeamsService(ILocalDataService db, IAppSettings appSettings)
        {
            _db = db;
            _appSettings = appSettings;
        }


        public async Task<RetVal> sendTeamsMessage(string body, string title)
        {

            var ret = new RetVal { success = false, errorMessage = "Could not send the message" };
            try
            {
                var client = new TeamsHookClient();
                var card = new MessageCard();
                card.Text = body;
                card.Title = title;


                await client.PostAsync(_appSettings.teamsWebhook, card);
                ret.success = true;
                ret.errorMessage = null;
            }
            catch (Exception e)
            {

                ret.errorMessage = e.Message;
            }

            return ret;

        }

        public async Task<RetVal> sendTeamsLastImportDates()
        {


            var ret = new RetVal { success = false, errorMessage = "Could not send the message" };
            try
            {
                var client = new TeamsHookClient();
                var card = new MessageCard();
                card.Title = $"Last PEGA BIX file import dates for: {(DateTime.Today, 10).ToString().Substring(1, 10)}";
                var imp = await _db.getLastFileImportDates();
                if (!imp.success)
                {
                    card.Text = "Could not read the latest file import dates";
                }
                else
                {
                    var lst = imp.returnVal!;
                    var tbl = _convertRecordsToHtml<spIMPORT_GetLastFileImportDates>(lst);
                    card.Text = tbl;
                }



                var result = await client.PostAsync(_appSettings.teamsWebhook, card);
                if (!result.IsSuccessStatusCode)
                {
                    ret.success = false;
                    ret.errorMessage = result.ReasonPhrase;
                    return ret;
                }
                ret.success = true;
                ret.errorMessage = null;
            }
            catch (Exception e)
            {

                ret.errorMessage = e.Message;
            }

            return ret;

        }


        public async Task<RetVal> sendTeamsLastKpiDateValues()
        {


            var ret = new RetVal { success = false, errorMessage = "Could not send the message" };
            try
            {
                var client = new TeamsHookClient();
                var card = new MessageCard();
                card.Title = $"Last PEGA BIX KPI dates values: {(DateTime.Today, 10).ToString().Substring(1, 10)}";
                var imp = await _db.getLastKpiDateValues();
                if (!imp.success)
                {
                    card.Text = "Could not read the latest file kpi values";
                }
                else
                {
                    var lst = imp.returnVal!;
                    var tbl = _convertRecordsToHtmlWithIsDifferent<spIMPORT_GetLast2KpiDateValues>(lst);
                    card.Text = tbl;


                    //var grpIds = lst.Select(r => r.groupId).Distinct();
                    //foreach (int? grpId in grpIds) {
                    //    var records = lst.Where(r => r.groupId == grpId).Select(c => c.fileName && c.status && c.currentDate && c.).ToList();
                    //    var tbl = _convertRecordsToHtml<spIMPORT_GetLastKpiDateValues>(records);
                    //    card.Text = tbl;
                    //}

                    var result = await client.PostAsync(_appSettings.teamsWebhook, card);
                    if (!result.IsSuccessStatusCode)
                    {
                        ret.success = false;
                        ret.errorMessage = result.ReasonPhrase;
                        return ret;
                    }


                }


                ret.success = true;
                ret.errorMessage = null;
            }
            catch (Exception e)
            {

                ret.errorMessage = e.Message;
            }

            return ret;

        }

        protected string _convertRecordsToHtml<T>(List<T> records)
        {
            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);


            // internal css is not supported by teams
            // only inline is supported
            StringBuilder strHTMLBuilder = new StringBuilder();
            strHTMLBuilder.Append("<html >");
            strHTMLBuilder.Append("<head>");
            strHTMLBuilder.Append("</head>");
            strHTMLBuilder.Append("<body>");
            strHTMLBuilder.Append("<table style='border-collapse: collapse; border-spacing: 1; border-radius: 12px; overflow: hidden; margin: 25px 0;font-size: 0.9em;font-family: sans-serif;min-width: 400px;box-shadow: 0 0 20px rgba(0, 0, 0, 0.15);'>");
            strHTMLBuilder.Append("<thead><tr style='background-color: #009879;color: #ffffff;text-align: left;font-size: 1.2em;'>");
            foreach (PropertyInfo prop in props)
            {
                var ttl = prop.Name;
                if (ttl.Substring(0, 1) == "_") ttl = ttl.Substring(1);
                strHTMLBuilder.Append("<td style='padding: 7px 15px;'><b>");
                strHTMLBuilder.Append(ttl);
                strHTMLBuilder.Append("</b></td>");
            }
            strHTMLBuilder.Append("</tr></thead><tbody>");


            foreach (T rec in records)
            {
                strHTMLBuilder.Append("<tr>");
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    if (props[i].PropertyType == typeof(int?))
                    {
                        strHTMLBuilder.Append("<td style='padding: 7px 15px;text-align: center;'>");
                    }
                    else
                    {
                        strHTMLBuilder.Append("<td style='padding: 7px 15px;'>");
                    }

                    var val = props[i].GetValue(rec!, null) ?? "";
                    strHTMLBuilder.Append(val);
                    strHTMLBuilder.Append("</td>");
                }
                strHTMLBuilder.Append("</tr>");
            }
            //Close tags.
            strHTMLBuilder.Append("</tbody></table>");
            strHTMLBuilder.Append("</body>");
            strHTMLBuilder.Append("</html>");
            string Htmltext = strHTMLBuilder.ToString();
            return Htmltext;
        }

        protected string _convertRecordsToHtmlWithIsDifferent<T>(List<T> records)
        {

            // be sure that the isDifferent column is the first column of T

            PropertyInfo[] props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);


            // internal css is not supported by teams
            // only inline is supported
            StringBuilder strHTMLBuilder = new StringBuilder();
            strHTMLBuilder.Append("<html >");
            strHTMLBuilder.Append("<head>");
            strHTMLBuilder.Append("</head>");
            strHTMLBuilder.Append("<body>");
            strHTMLBuilder.Append("<table style='border-collapse: collapse; border-spacing: 1; border-radius: 12px; overflow: hidden; margin: 25px 0;font-size: 0.9em;font-family: sans-serif;min-width: 400px;box-shadow: 0 0 20px rgba(0, 0, 0, 0.15);'>");
            strHTMLBuilder.Append("<thead><tr style='background-color: #009879;color: #ffffff;text-align: left;font-size: 1.2em;'>");
            foreach (PropertyInfo prop in props)
            {
                if (prop.Name == "isDifferent") continue;
                var ttl = prop.Name;
                if (ttl.Substring(0, 1) == "_") ttl = ttl.Substring(1);
                strHTMLBuilder.Append("<td style='padding: 7px 15px;'><b>");
                strHTMLBuilder.Append(ttl);
                strHTMLBuilder.Append("</b></td>");
            }
            strHTMLBuilder.Append("</tr></thead><tbody>");


            foreach (T rec in records)
            {

                bool isDiff = false;
                if (props[0].PropertyType == typeof(bool?))
                {
                    isDiff = (bool)props[0].GetValue(rec, null)!;
                }

                if (isDiff)
                {
                    strHTMLBuilder.Append("<tr style='background-color: #D7AB1F;color: #ffffff'>");
                }
                else
                {
                    strHTMLBuilder.Append("<tr>");
                }



                var values = new object[props.Length];
                for (int i = 1; i < props.Length; i++)
                {

                    int number;
                    var val = props[i].GetValue(rec!, null) ?? "--";
                    // if val is a number, center it in the cell
                    if (int.TryParse(val.ToString(),out number)== true)
                    {
                        strHTMLBuilder.Append("<td style='padding: 7px 15px;text-align: center;'>");
                    }
                    else
                    {
                        strHTMLBuilder.Append("<td style='padding: 7px 15px;'>");
                    }


                    
                    strHTMLBuilder.Append(val);
                    strHTMLBuilder.Append("</td>");
                }
                strHTMLBuilder.Append("</tr>");
            }
            //Close tags.
            strHTMLBuilder.Append("</tbody></table>");
            //strHTMLBuilder.Append("<a href= \"file://///jpfile01/#FILES//DATA_INTELLIGENCE_PEGA//data/P/archive/AG_NL_Claims_Data_BIXKPI_BIXExtract_Full_230413.xml\">Link to file</a>");
            strHTMLBuilder.Append("</body>");
            strHTMLBuilder.Append("</html>");
            string Htmltext = strHTMLBuilder.ToString();
            return Htmltext;
        }

    }
}
