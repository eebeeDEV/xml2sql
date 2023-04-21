/****** Object:  UserDefinedTableType [dbo].[utp_IMPORT_TableList]    Script Date: 21/04/2023 11:20:19 ******/
CREATE TYPE [dbo].[utp_IMPORT_TableList] AS TABLE(
	[tableName] [nvarchar](200) NULL
)
GO
/****** Object:  UserDefinedTableType [dbo].[utp_IMPORT_TableModel]    Script Date: 21/04/2023 11:20:19 ******/
CREATE TYPE [dbo].[utp_IMPORT_TableModel] AS TABLE(
	[columnName] [nvarchar](200) NULL,
	[columnType] [nvarchar](30) NULL,
	[columnSize] [int] NULL
)
GO
/****** Object:  UserDefinedFunction [dbo].[ufImport_CalcLookupId]    Script Date: 21/04/2023 11:20:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- this function returns the substring by searching for the first digit
-- and convert it to an int
-- the input string must end with the is
-- ex: 'AG_NL_CLAIMS_WORK_IN_100012'
CREATE FUNCTION [dbo].[ufImport_CalcLookupId] 
(
	  @prefix varchar(20)
	 ,@value varchar(80)
)
RETURNS varchar(50)
WITH SCHEMABINDING
AS
BEGIN
	Declare 
		 @retval varchar(50) = ''

	SELECT @retval = SUBSTRING(@value, LEN(@prefix)+2, 80)

	RETURN @retval
END


GO
/****** Object:  UserDefinedFunction [dbo].[ufImport_CalcLookupId2]    Script Date: 21/04/2023 11:20:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[ufImport_CalcLookupId2] 
(
	  @prefix varchar(20)
	 ,@value varchar(80)
)
RETURNS varchar(50)
WITH SCHEMABINDING
AS
BEGIN
	Declare 
		 @retval varchar(50) = ''

	SELECT @retval = SUBSTRING(@value, LEN(@prefix)+2, 80)

	RETURN @retval
END
GO
/****** Object:  UserDefinedFunction [dbo].[ufImport_CalcLookupIdWithoutPrefix]    Script Date: 21/04/2023 11:20:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- this function returns the substring by searching for the first digit
-- and convert it to an int
-- the input string must end with the is
-- ex: 'AG_NL_CLAIMS_WORK_IN_100012'
CREATE FUNCTION [dbo].[ufImport_CalcLookupIdWithoutPrefix] 
(
	 @value varchar(80)
)
RETURNS varchar(50)
WITH SCHEMABINDING
AS
BEGIN
	Declare 
		 @retval varchar(50) = ''


	Select @retval = SUBSTRING(@value, PATINDEX('%[0-9]%', @value), 20) 
	RETURN @retval
END


GO
/****** Object:  Table [dbo].[IMPORT_BypassObjects]    Script Date: 21/04/2023 11:20:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IMPORT_BypassObjects](
	[bypassId] [int] IDENTITY(1,1) NOT NULL,
	[whichCase] [varchar](50) NULL,
	[tableName] [varchar](100) NULL,
	[bypassTable] [bit] NOT NULL,
	[columnName] [varchar](100) NULL,
 CONSTRAINT [PK_IMPORT_BypassObjects] PRIMARY KEY CLUSTERED 
(
	[bypassId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IMPORT_CounterRun]    Script Date: 21/04/2023 11:20:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IMPORT_CounterRun](
	[RunId] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IMPORT_FileCatalog]    Script Date: 21/04/2023 11:20:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IMPORT_FileCatalog](
	[fileId] [int] IDENTITY(1,1) NOT NULL,
	[fileBaseName] [varchar](100) NULL,
	[fileShortName] [varchar](30) NULL,
	[fileKpiTypeClass] [varchar](100) NULL,
	[fileKpiItemTable] [varchar](100) NULL,
	[fileIsActive] [bit] NULL,
	[fileAdded] [datetime] NULL,
	[fileLastImportFileDate] [date] NULL,
	[fileLastBypassDate] [date] NULL,
	[fileLastImportDateTime] [datetime] NULL,
	[displOrder] [int] NULL,
	[fileGroupId] [int] NULL,
 CONSTRAINT [PK_FileCatalog] PRIMARY KEY CLUSTERED 
(
	[fileId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IMPORT_Kpi_item]    Script Date: 21/04/2023 11:20:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IMPORT_Kpi_item](
	[recordId] [int] IDENTITY(1,1) NOT NULL,
	[dins] [datetime] NULL,
	[BRRDDEL] [bit] NULL,
	[fileName] [varchar](200) NULL,
	[fileDate] [date] NULL,
	[runId] [int] NULL,
	[isBaseLine] [bit] NULL,
	[id] [varchar](100) NULL,
	[ATStatusLabel] [varchar](40) NULL,
	[caseTypeClass] [varchar](60) NULL,
	[Count] [int] NULL,
	[pxCommitDateTime] [datetime2](7) NULL,
	[pxObjClass] [varchar](40) NULL,
	[pxSaveDateTime] [datetime2](7) NULL,
	[pyGUID] [varchar](40) NULL,
	[pzInsKey] [varchar](100) NULL,
 CONSTRAINT [PK_IMPORT_Recon_item] PRIMARY KEY CLUSTERED 
(
	[recordId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IMPORT_Kpi_values]    Script Date: 21/04/2023 11:20:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IMPORT_Kpi_values](
	[recordId] [int] IDENTITY(1,1) NOT NULL,
	[dins] [datetime] NULL,
	[BRRDDEL] [bit] NULL,
	[fileId] [int] NULL,
	[fileName] [varchar](100) NULL,
	[fileDate] [date] NULL,
	[runId] [int] NULL,
	[kpiFileName] [varchar](100) NULL,
	[kpiFileDate] [date] NULL,
	[caseTypeClass] [varchar](60) NULL,
	[statusWork] [varchar](60) NULL,
	[countBaseLine] [int] NULL,
	[countFromTable] [int] NULL,
	[countFromKpiFile] [int] NULL,
	[countDifference] [int] NULL,
 CONSTRAINT [PK_IMPORT_Recap_values] PRIMARY KEY CLUSTERED 
(
	[recordId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IMPORT_TableChanges]    Script Date: 21/04/2023 11:20:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IMPORT_TableChanges](
	[tableChangeId] [int] IDENTITY(1,1) NOT NULL,
	[createDate] [datetime] NOT NULL,
	[whichCase] [varchar](20) NULL,
	[tableName] [varchar](100) NULL,
	[columnName] [varchar](200) NULL,
	[columnTypeTable] [varchar](30) NULL,
	[columnTypeBixFile] [varchar](30) NULL,
	[objectType] [varchar](15) NULL,
	[actionType] [varchar](15) NULL,
	[actionTypeId] [int] NULL,
	[fileName] [varchar](100) NULL,
	[fileDate] [datetime2](7) NULL,
	[runId] [int] NULL,
 CONSTRAINT [PK_IMPORT_TableChanges] PRIMARY KEY CLUSTERED 
(
	[tableChangeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IMPORT_TableChangesActions]    Script Date: 21/04/2023 11:20:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IMPORT_TableChangesActions](
	[actionTypeId] [int] NOT NULL,
	[actionType] [varchar](30) NULL,
 CONSTRAINT [PK_IMPORT_TableChangesActions] PRIMARY KEY CLUSTERED 
(
	[actionTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[IMPORT_BypassObjects] ADD  CONSTRAINT [DF_IMPORT_BypassObjects_bypassTable]  DEFAULT ((0)) FOR [bypassTable]
GO
ALTER TABLE [dbo].[IMPORT_FileCatalog] ADD  CONSTRAINT [DF_IMPORT_FileCatalog_fileIsActive]  DEFAULT ((0)) FOR [fileIsActive]
GO
ALTER TABLE [dbo].[IMPORT_FileCatalog] ADD  CONSTRAINT [DF_FileCatalog_fileAdded]  DEFAULT (getdate()) FOR [fileAdded]
GO
ALTER TABLE [dbo].[IMPORT_Kpi_item] ADD  CONSTRAINT [DF_IMPORT_Recon_item_dins]  DEFAULT (getdate()) FOR [dins]
GO
ALTER TABLE [dbo].[IMPORT_Kpi_item] ADD  CONSTRAINT [DF_IMPORT_Recon_item_BRRDDEL]  DEFAULT ((0)) FOR [BRRDDEL]
GO
ALTER TABLE [dbo].[IMPORT_Kpi_item] ADD  CONSTRAINT [DF_IMPORT_Recap_item_isBaseLine]  DEFAULT ((0)) FOR [isBaseLine]
GO
ALTER TABLE [dbo].[IMPORT_Kpi_values] ADD  CONSTRAINT [DF_IMPORT_Recap_values_dins]  DEFAULT (getdate()) FOR [dins]
GO
ALTER TABLE [dbo].[IMPORT_TableChanges] ADD  CONSTRAINT [DF_IMPORT_TableChanges_createDate]  DEFAULT (getdate()) FOR [createDate]
GO
ALTER TABLE [dbo].[IMPORT_TableChanges]  WITH CHECK ADD  CONSTRAINT [FK_IMPORT_TableChanges_IMPORT_TableChangesActions] FOREIGN KEY([actionTypeId])
REFERENCES [dbo].[IMPORT_TableChangesActions] ([actionTypeId])
GO
ALTER TABLE [dbo].[IMPORT_TableChanges] CHECK CONSTRAINT [FK_IMPORT_TableChanges_IMPORT_TableChangesActions]
GO
ALTER TABLE [dbo].[IMPORT_TableChangesActions]  WITH CHECK ADD  CONSTRAINT [FK_IMPORT_TableChangesActions_IMPORT_TableChangesActions] FOREIGN KEY([actionTypeId])
REFERENCES [dbo].[IMPORT_TableChangesActions] ([actionTypeId])
GO
ALTER TABLE [dbo].[IMPORT_TableChangesActions] CHECK CONSTRAINT [FK_IMPORT_TableChangesActions_IMPORT_TableChangesActions]
GO
/****** Object:  StoredProcedure [dbo].[spIMPORT_GetFileCatalog]    Script Date: 21/04/2023 11:20:19 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[spIMPORT_GetFileCatalog]
	 @success bit OUT
	,@ErrorMessage nvarchar(MAX) OUTPUT
 AS
 
BEGIN

	BEGIN TRY

		SELECT * from dbo.IMPORT_FileCatalog
		SELECT @Success = 1, @ErrorMessage = ''

	END TRY

	BEGIN CATCH	
		SELECT @Success = 0, @ErrorMessage = ERROR_MESSAGE()
	END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[spIMPORT_GetLast10KpiDateValues]    Script Date: 21/04/2023 11:20:19 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[spIMPORT_GetLast10KpiDateValues]
	 @success bit OUT
	,@ErrorMessage nvarchar(MAX) OUTPUT
 AS
 
BEGIN

	BEGIN TRY

		WITH lastDays_cte
		AS (
			SELECT TOP 10 fileDate
			FROM dbo.IMPORT_Kpi_values
			WHERE kpiFileName IS NOT NULL
			GROUP BY fileDate
			ORDER BY fileDate DESC
			)
			,mDate_cte
		AS (
			SELECT mDate = MAX(fileDate)
			FROM dbo.IMPORT_Kpi_values
			)
		SELECT 
			 c.fileGroupId AS groupId
			,c.fileShortName AS fileName
			,t.statusWork AS status
			,CONVERT (VARCHAR, md.mDate, 105) as currentDate
			,ISNULL(CAST(SUM(CASE WHEN t.fileDate = md.mDate THEN t.countDifference ELSE NULL END) AS varchar(10)), '--') AS today
			,ISNULL(CAST(SUM(CASE WHEN t.fileDate = DATEADD(day, -1, md.mDate) THEN t.countDifference ELSE NULL END)AS VARCHAR(10)), '--') AS [1dayAgo]
			,ISNULL(CAST(SUM(CASE WHEN t.fileDate = DATEADD(day, -2, md.mDate) THEN t.countDifference ELSE NULL END)AS VARCHAR(10)), '--') AS [2daysAgo]
			,ISNULL(CAST(SUM(CASE WHEN t.fileDate = DATEADD(day, -3, md.mDate) THEN t.countDifference ELSE NULL END)AS VARCHAR(10)), '--') AS [3daysAgo]
			,ISNULL(CAST(SUM(CASE WHEN t.fileDate = DATEADD(day, -4, md.mDate) THEN t.countDifference ELSE NULL END)AS VARCHAR(10)), '--') AS [4daysAgo]
			,ISNULL(CAST(SUM(CASE WHEN t.fileDate = DATEADD(day, -5, md.mDate) THEN t.countDifference ELSE NULL END)AS VARCHAR(10)), '--') AS [5daysAgo]
			,ISNULL(CAST(SUM(CASE WHEN t.fileDate = DATEADD(day, -6, md.mDate) THEN t.countDifference ELSE NULL END)AS VARCHAR(10)), '--') AS [6daysAgo]
			,ISNULL(CAST(SUM(CASE WHEN t.fileDate = DATEADD(day, -7, md.mDate) THEN t.countDifference ELSE NULL END)AS VARCHAR(10)), '--') AS [7daysAgo]
			,ISNULL(CAST(SUM(CASE WHEN t.fileDate = DATEADD(day, -8, md.mDate) THEN t.countDifference ELSE NULL END)AS VARCHAR(10)), '--') AS [8daysAgo]
			,ISNULL(CAST(SUM(CASE WHEN t.fileDate = DATEADD(day, -9, md.mDate) THEN t.countDifference ELSE NULL END)AS VARCHAR(10)), '--') AS [9daysAgo]
		FROM dbo.IMPORT_Kpi_values AS t
		INNER JOIN lastDays_cte AS ltd ON t.fileDate = ltd.fileDate
		INNER JOIN dbo.IMPORT_FileCatalog AS c ON t.caseTypeClass = c.fileKpiTypeClass
		CROSS APPLY mDate_cte AS md
		WHERE kpiFileName IS NOT NULL
		GROUP BY
			 c.fileGroupId
			,c.fileShortName
			,c.displOrder
			,t.statusWork
			,md.mDate
		ORDER BY
			 c.fileGroupId
			,c.displOrder
			,c.fileShortName
			,t.statusWork		
		
		SELECT @Success = 1, @ErrorMessage = ''

	END TRY

	BEGIN CATCH	
		SELECT @Success = 0, @ErrorMessage = ERROR_MESSAGE()
	END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[spIMPORT_GetLast2KpiDateValues]    Script Date: 21/04/2023 11:20:19 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[spIMPORT_GetLast2KpiDateValues]
	 @success bit OUT
	,@ErrorMessage nvarchar(MAX) OUTPUT
 AS
 
BEGIN

	BEGIN TRY

		WITH lastDays_cte
		AS (
			SELECT TOP 2 fileDate
			FROM dbo.IMPORT_Kpi_values
			WHERE kpiFileName IS NOT NULL
			GROUP BY fileDate
			ORDER BY fileDate DESC
			)
			,mDate_cte
		AS (
			SELECT mDate = MAX(fileDate)
			FROM dbo.IMPORT_Kpi_values
			)
		SELECT fileName, status, currentDate, today, yesterday, CAST(CASE WHEN today <> yesterday THEN 1 ELSE 0 END AS bit) AS isDifferent FROM 
		(
		SELECT TOP 100 PERCENT 
			 c.fileShortName AS fileName
			,t.statusWork AS status
			,CONVERT (VARCHAR, md.mDate, 105) as currentDate
			,SUM(CASE WHEN t.fileDate = md.mDate THEN t.countDifference ELSE NULL END) AS today
			,SUM(CASE WHEN t.fileDate = DATEADD(day, -1, md.mDate) THEN t.countDifference ELSE NULL END) AS yesterday
		FROM dbo.IMPORT_Kpi_values AS t
		INNER JOIN lastDays_cte AS ltd ON t.fileDate = ltd.fileDate
		INNER JOIN dbo.IMPORT_FileCatalog AS c ON t.caseTypeClass = c.fileKpiTypeClass
		CROSS APPLY mDate_cte AS md
		WHERE kpiFileName IS NOT NULL
		GROUP BY
			 c.fileGroupId
			,c.fileShortName
			,c.displOrder
			,t.statusWork
			,md.mDate
		ORDER BY
			 c.fileGroupId
			,c.displOrder
			,c.fileShortName
			,t.statusWork
		) AS TBL
		
		SELECT @Success = 1, @ErrorMessage = ''

	END TRY

	BEGIN CATCH	
		SELECT @Success = 0, @ErrorMessage = ERROR_MESSAGE()
	END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[spIMPORT_GetLastFileImportDates]    Script Date: 21/04/2023 11:20:19 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[spIMPORT_GetLastFileImportDates]
	 @success bit OUT
	,@ErrorMessage nvarchar(MAX) OUTPUT
 AS
 
BEGIN

	BEGIN TRY
		SELECT 
			 fileShortName as fileName
			,CONVERT (VARCHAR, fileLastImportFileDate, 105) as lastDate
			,CASE WHEN fileLastBypassDate IS NULL 
			THEN 
				CAST(DATEDIFF(DAY, fileLastImportFileDate, CAST(GETDATE() as date)) as varchar(4)) 
			ELSE 
				'BYPASSED since ' + CAST(DATEDIFF(DAY, fileLastBypassDate, CAST(GETDATE() as date)) as varchar(4)) + ' days' 
			END as daysPassed
		FROM 
			dbo.IMPORT_FileCatalog
		WHERE
			fileLastImportFileDate IS NOT NULL
		ORDER BY 
			displOrder
		
		
		SELECT @Success = 1, @ErrorMessage = ''

	END TRY

	BEGIN CATCH	
		SELECT @Success = 0, @ErrorMessage = ERROR_MESSAGE()
	END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[spIMPORT_GetNextFileInfo]    Script Date: 21/04/2023 11:20:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spIMPORT_GetNextFileInfo]
	 @FileBaseName varchar(100)
	,@Success bit OUT
	,@ErrorMessage nvarchar(MAX) OUTPUT

AS

BEGIN
	SET NOCOUNT ON;

	DECLARE
		 @NextRunId INT
		,@NextFileName varchar(100)
		,@NextDate date
		,@NoFileToImport bit
	
	BEGIN TRY
	
		IF NOT EXISTS(SELECT * FROM dbo.IMPORT_FileCatalog WHERE fileBaseName = @FileBaseName)
		BEGIN
			SELECT @Success = 0, @ErrorMessage = ''
			RETURN
		END
	
		DECLARE 
			 @lastDate date
		

	
		-- if the file was bypassed the last time
		-- return as if the last file was the one from 2 days ago
		-- this way the program will try to check for the file from yesterday (last file possible)
		-- else, return the last import date
		SELECT 
			@lastDate = fileLastImportFileDate
		FROM 
			dbo.IMPORT_FileCatalog 
		WHERE 
			fileBaseName = @FileBaseName 
		SELECT @NextDate = DATEADD(DAY, 1, @lastDate)

		SELECT @NextFileName = @FileBaseName + '_' + FORMAT(@NextDate, 'yyMMdd') + '.xml'
		-- increase the runId first
		UPDATE dbo.IMPORT_CounterRun SET RunId = RunId + 1 	
		SELECT @NextRunId = RunId FROM dbo.IMPORT_CounterRun
	
		-- the last file date is allways current date -1
		IF @NextDate > DATEADD(DAY, -1, GETDATE())
			SELECT @NoFileToImport = 1
		ELSE
			SELECT @NoFileToImport = 0
	
		SELECT @NextDate as nextDate, @NextFileName as nextFileName, @NextRunId as nextRunId, @NoFileToImport as noFileToImport

		SELECT @Success = 1, @ErrorMessage = ''

	END TRY

	BEGIN CATCH
		SELECT null nextDate, null nextFileName, null nextRunId, null as noFileToImport
		SELECT @Success = 0, @ErrorMessage = ERROR_MESSAGE()
	END CATCH

END
GO
/****** Object:  StoredProcedure [dbo].[spIMPORT_GetTableFields]    Script Date: 21/04/2023 11:20:19 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[spIMPORT_GetTableFields]
	 @TableName varchar(400)
	,@TextFieldsOnly bit
	,@Success bit OUTPUT
	,@ErrorMessage nvarchar(MAX) OUTPUT

 AS
 
 BEGIN

	BEGIN TRY 
		SELECT @TableName = REPLACE(REPLACE(REPLACE(@TableName, '[', ''), ']', ''),'''', '');
	
		-- remove the schema from the name if it is added
		DECLARE @pnt int = CHARINDEX('.',@TableName)
		if(@pnt > 0)
		BEGIN
			SELECT @TableName = SUBSTRING(@TableName, @pnt + 1, 400)
		END

		IF @TextFieldsOnly = 1
		BEGIN
 			SELECT 
				 c.name AS fieldName
				,c.system_type_id as fieldTypeId
				,ft.name as fieldType
				,c.max_length as fieldSize
			FROM 
				sys.columns AS c
				INNER JOIN sys.objects AS o ON c.object_id = o.object_id
				INNER JOIN sys.types as ft on c.system_type_id = ft.system_type_id
			WHERE 
				o.type = 'U' 
				and o.name = @TableName
				and c.system_type_id in (35,99,231,239,167,175)
		END
		ELSE
		BEGIN
 			SELECT 
				 c.name AS fieldName
				,c.system_type_id as fieldTypeId
				,ft.name as fieldType
				,c.max_length as fieldSize
			FROM 
				sys.columns AS c
				INNER JOIN sys.objects AS o ON c.object_id = o.object_id
				INNER JOIN sys.types as ft on c.system_type_id = ft.system_type_id
			WHERE 
				o.type = 'U' 
				and o.name = @TableName
		END

		SELECT @Success = 1, @ErrorMessage = ''
	END TRY

	BEGIN CATCH	
		SELECT @Success = 0, @ErrorMessage = ERROR_MESSAGE()
	END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[spIMPORT_IsTableBypassed]    Script Date: 21/04/2023 11:20:19 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[spIMPORT_IsTableBypassed]	
	 @TableName varchar(100)
	,@IsBypassed bit OUTPUT
	,@Success bit OUTPUT
	,@ErrorMessage nvarchar(MAX) OUTPUT

 AS
 
BEGIN

	-- check ik the table should be bypassed
	IF EXISTS(SELECT * FROM dbo.IMPORT_BypassObjects as b WHERE b.bypassTable = 1 and b.tableName = @TableName)
		SELECT @Success = 1, @IsBypassed = 1, @ErrorMessage = ''
	ELSE
	BEGIN		
		SELECT @Success = 1, @IsBypassed = 0, @ErrorMessage = ''
	END

END

GO
/****** Object:  StoredProcedure [dbo].[spIMPORT_PostProcess]    Script Date: 21/04/2023 11:20:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spIMPORT_PostProcess]
	 @FileBaseName varchar(100)
	,@FileName varchar(100)
	,@FileDate date 
	,@RunId int
	,@SchemaName varchar(20)
	,@TablePrefix varchar(60)	
	,@BrrddelPartitionFields varchar(400)
	,@BrrddelOrderByFields varchar(400)	
	,@Success bit OUT
	,@ErrorMessage nvarchar(MAX) OUTPUT

AS

BEGIN
	SET NOCOUNT ON;

	DECLARE
		 @NextRunId INT = 0
		,@sql nvarchar(MAX)
		,@tran varchar(3) = 'TR1'

	BEGIN TRANSACTION @tran
	BEGIN TRY

		-- check for SQL injection
		IF CHARINDEX(';', @SchemaName) > 0 RETURN
		IF CHARINDEX(';', @TablePrefix) > 0 RETURN

	
		IF NOT EXISTS(SELECT * FROM dbo.IMPORT_FileCatalog WHERE fileBaseName = @FileBaseName)
		BEGIN
			SELECT @Success = 0, @ErrorMessage = 'FileBaseName does not exists in catalog'
			COMMIT TRANSACTION @tran
			RETURN
		END
	
		DECLARE 
			 @lastDate date
		
		-- set the last importdate for the FileBaseName
		UPDATE dbo.IMPORT_FileCatalog SET fileLastImportFileDate = @FileDate, fileLastBypassDate = null, fileLastImportDateTime = GETDATE() WHERE fileBaseName = @FileBaseName

		-- set the BRRDDEL flag
		-- check if the _Item table exists
		IF EXISTS(Select * from SYS.TABLES as t inner join SYS.schemas as s ON t.schema_id = s.schema_id WHERE t.name = @TablePrefix + 'Item' and s.name = @SchemaName)
		BEGIN	
			-- set first all BRRDDEL = 0
			Select @sql = 'UPDATE ' + @SchemaName + '.' + @TablePrefix + 'Item SET BRRDDEL = 0 WHERE BRRDDEL = 1'
			exec dbo.sp_executesql @sql
			
			-- calc the new BRRDDEL
			Select @sql = 'UPDATE tbl SET BRRDDEL = 1 FROM ' +
				'(SELECT BRRDDEL, Row_Number() OVER (Partition BY ' + @BrrddelPartitionFields + 
				' ORDER BY ' + @BrrddelOrderByFields + ' desc, recordId desc) AS Idx ' + 
				' FROM ' + @SchemaName + '.' + @TablePrefix + 'item) as tbl where Idx > 1'

			--print @sql
			exec dbo.sp_executesql @sql
		END
		ELSE
		BEGIN
			SELECT @Success = 0, @ErrorMessage = 'Could not find the ' + @TablePrefix + 'Item table'
			COMMIT TRANSACTION @tran
			RETURN
		END

		-- level up the ID's from this run
		--EXEC dbo.spIMPORT_PostProcess_LevelUpIds @TablePrefix, @SchemaName, @Success OUT, @ErrorMessage OUT

		-- calc the statusWork counts for this file
		EXEC dbo.spIMPORT_PostProcess_CalcKpiFromTable @FileBaseName, @FileName, @FileDate, @RunId, @SchemaName, @TablePrefix, @Success OUT,@ErrorMessage OUT


		COMMIT TRANSACTION @tran

		SELECT @Success = 1, @ErrorMessage = ''
	END TRY

	BEGIN CATCH
		ROLLBACK TRANSACTION @tran
		SELECT @Success = 0, @ErrorMessage = ERROR_MESSAGE() + ' | SQL: ' + ISNULL(@sql, '')
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[spIMPORT_PostProcess_CalcKpiFromKpiFile]    Script Date: 21/04/2023 11:20:19 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[spIMPORT_PostProcess_CalcKpiFromKpiFile]
	 @Success bit OUTPUT
	,@ErrorMessage nvarchar(MAX) OUTPUT

 AS
 
 BEGIN

	BEGIN TRY 



		-- update the IMPORT_Kpi_values table
		-- get the counts + baseline from the dbo.IMPORT_Kpi_item 
		-- with the same date, status and fileId (fileKpiTypeClass linked to IMPORT_FileCatalog)

		-- as only inner joins are used, if either a baseline, lastcount or Kpi_values would be missing
		-- no record will be updated, and as such be visible
		UPDATE v
		SET 
			 countBaseLine = base.countBaseLine
			,countFromKpiFile = lCount.countFromKpiFile
			,countDifference = ISNULL(v.countFromTable, 0) - (ISNULL(lCount.countFromKpiFile, 0) + ISNULL(base.countBaseLine, 0))
			,caseTypeClass = c.fileKpiTypeClass
			,kpiFileName = lCount.kpiFileName
			,kpiFileDate = lCount.kpiFileDate
		FROM (
			SELECT 
				 ATStatusLabel
				,caseTypeClass
				,Count AS countBaseLine
			FROM 
				dbo.IMPORT_Kpi_item
			WHERE 
				isBaseLine = 1
			) AS base
		INNER JOIN (
			SELECT 
				 ATStatusLabel
				,caseTypeClass
				,Count AS countFromKpiFile
				,CAST(pxSaveDateTime AS DATE) AS fileDate
				,fileName AS kpiFileName
				,fileDate AS kpiFileDate
			FROM 
				dbo.IMPORT_Kpi_item
			) AS lCount ON base.ATStatusLabel = lCount.ATStatusLabel AND base.caseTypeClass = lCount.caseTypeClass
		INNER JOIN dbo.IMPORT_FileCatalog AS c ON lCount.caseTypeClass = c.fileKpiTypeClass
		INNER JOIN dbo.IMPORT_Kpi_values AS v ON c.fileId = v.fileId AND v.statusWork = lCount.ATStatusLabel AND v.fileDate = lCount.fileDate
		WHERE
			v.kpiFileName is null


		SELECT @Success = 1, @ErrorMessage = ''
	END TRY

	BEGIN CATCH	
		SELECT @Success = 0, @ErrorMessage = ERROR_MESSAGE()
	END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[spIMPORT_PostProcess_CalcKpiFromTable]    Script Date: 21/04/2023 11:20:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spIMPORT_PostProcess_CalcKpiFromTable]
	 @FileBaseName varchar(100)
	,@FileName varchar(100)
	,@FileDate date 
	,@RunId int 	
	,@SchemaName varchar(30)
	,@TablePrefix varchar(60)	
	,@Success bit OUT
	,@ErrorMessage nvarchar(MAX) OUTPUT

AS

BEGIN
	SET NOCOUNT ON;

	DECLARE
		 @NextRunId INT = 0
		,@sql nvarchar(MAX)
		,@tran varchar(3) = 'TR1'

	BEGIN TRANSACTION @tran
	BEGIN TRY

		-- check for SQL injection
		IF CHARINDEX(';', @TablePrefix) > 0 RETURN

		-- check if the file exist in the catalog
		IF NOT EXISTS(SELECT * FROM dbo.IMPORT_FileCatalog WHERE fileBaseName = @FileBaseName)
		BEGIN
			SELECT @Success = 0, @ErrorMessage = 'FileBaseName does not exists in catalog'
			COMMIT TRANSACTION @tran 
			RETURN
		END

		-- check if the status column is present in the table
		-- if not, impossible to calculate the status totals
		IF NOT EXISTS(SELECT * FROM sys.tables as t inner join sys.columns as c on t.object_id = c.object_id WHERE t.name = @TablePrefix + 'item' and c.name = 'pyStatusWork')
		BEGIN
			SELECT @Success = 1, @ErrorMessage = 'NO Status column'
			COMMIT TRANSACTION @tran
			RETURN
		END
		
		DECLARE 
			 @fileId int
		
		-- set the last importdate for the FileBaseName
		Select @fileId = fileId FROM dbo.IMPORT_FileCatalog WHERE fileBaseName = @FileBaseName
		
		-- set the BRRDDEL flag
		-- check if the _Item table exists
		IF EXISTS(Select * from SYS.TABLES as t inner join SYS.schemas as s ON t.schema_id = s.schema_id WHERE t.name = @TablePrefix + 'Item' and s.name = @SchemaName)
		BEGIN	
			-- set first all BRRDDEL = 0
			UPDATE dbo.IMPORT_Kpi_values SET BRRDDEL = 0 WHERE BRRDDEL = 1 AND fileId = @fileId

			-- delete if records for this runid and fileId exists already 
			DELETE FROM dbo.IMPORT_Kpi_values WHERE fileId = @fileId AND runId = @RunId
			
			-- build the SQL
			-- get the counts per statusWork for this fileId
			SELECT @sql = 'INSERT INTO dbo.IMPORT_Kpi_values(fileId,BRRDDEL,fileName,fileDate,runId,statusWork,countFromTable) ' +
							'Select ' + CAST(@fileId as varchar(6)) + ',0 as BRRDDEL,''' + @FileName + ''',''' + CAST(@FileDate as varchar(10)) + ''',' + CAST(@RunId as varchar(6)) + ',pyStatusWork,COUNT(*) as tblCount ' + 
							'FROM dbo.' + @TablePrefix + 'item WHERE BRRDDEL = 0 GROUP BY pyStatusWork '						
			exec dbo.sp_executesql @sql			
			
			-- calc the new BRRDDEL
			UPDATE tbl SET BRRDDEL = 1 FROM 
				(SELECT BRRDDEL, Row_Number() OVER (Partition BY fileId, statusWork
				 ORDER BY fileDate desc, dins desc, recordId desc) AS Idx  
				 FROM dbo.IMPORT_Kpi_values where fileId = @fileId) as tbl where Idx > 1

		END
		ELSE
		BEGIN
			SELECT @Success = 0, @ErrorMessage = 'Could not find the ' + @TablePrefix + 'Item table'
			COMMIT TRANSACTION @tran
			RETURN
		END



		COMMIT TRANSACTION @tran

		SELECT @Success = 1, @ErrorMessage = ''
	END TRY

	BEGIN CATCH
		ROLLBACK TRANSACTION @tran
		SELECT @Success = 0, @ErrorMessage = ERROR_MESSAGE() + ' | SQL: ' + ISNULL(@sql, '')
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[spIMPORT_PostProcess_LevelUpIds]    Script Date: 21/04/2023 11:20:19 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[spIMPORT_PostProcess_LevelUpIds]	
	 @TablePrefix varchar(60)	
	,@Schema varchar(30)	
	,@Success bit OUT
	,@ErrorMessage nvarchar(MAX) OUTPUT
AS
BEGIN
	DECLARE 
		 @RunId int
		,@Params nvarchar(100) = '@RunId int OUTPUT'
		,@Sql nvarchar(MAX)
		,@tran varchar(3) = 'TR1'

	BEGIN TRANSACTION @tran
	BEGIN TRY
	
		IF CHARINDEX(';', @Schema) > 0 RETURN
		IF CHARINDEX(';', @TablePrefix) > 0 RETURN



		-- GET the last runId
		SELECT @Sql = 'SELECT @RunId = MAX(RunId) from ' + @schema + '.' + @TablePrefix + 'item'
		EXEC dbo.sp_executesql @Sql, @Params, @RunId OUT

		-- construct the update SQL statements
		-- we take the max(fld_id) of the previous runid and add it to the fld_id of the current runid
		-- we check the first fld_id = 0 to be sure that this runid has not been set yet
		-- we need to CAST the sql to varchar(MAX) so STRING_AGG to accept a result larger than 4000 chars
		SELECT
			@Sql = STRING_AGG(CAST('UPDATE i SET ' + c.name + ' = m.lastId + ' + c.name +  ' + 1 
			FROM ' + @Schema + '.' + t.name + ' as i
			CROSS APPLY (select Max(' + c.name +  ') as lastId from ' + @schema + '.' + t.name + ' where runId = ' + CAST((@RunId - 1) as varchar(6))  + ') as m
			CROSS APPLY (select Min(' + c.name +  ') as firstId from ' + @schema + '.' + t.name + ' where runId = ' + CAST(@RunId as varchar(6))  + ') as f
			WHERE i.runId  = ' + CAST(@runId as varchar(6)) + ' AND f.firstId = 0' as varchar(MAX)), ';')
		FROM 
			sys.columns as c
			inner join sys.tables as t on t.object_id = c.object_id
			inner join sys.schemas as s on t.schema_id = s.schema_id
		WHERE 
			t.name LIKE @TablePrefix + '%'
			and s.name = @Schema
			and c.name like '%[_]id'

		EXEC dbo.sp_executesql @Sql

		-- commit the transaction
		COMMIT TRANSACTION @tran

		SELECT @Success = 1, @ErrorMessage = ''


	END TRY

	BEGIN CATCH
		ROLLBACK TRANSACTION @tran
		SELECT @Success = 0, @ErrorMessage = ERROR_MESSAGE() + ' | SQL: ' + ISNULL(@sql, '')
	END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[spIMPORT_SaveBypassDate]    Script Date: 21/04/2023 11:20:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spIMPORT_SaveBypassDate]
	 @FileBaseName varchar(100)
	,@Success bit OUT
	,@ErrorMessage nvarchar(MAX) OUTPUT

AS

BEGIN
	-- this proc adds 1 day to the fileLastImportFileDate for this filebasename
	-- the app will verify if this date must be bypassed
	
	SET NOCOUNT ON;

	
	BEGIN TRY
		
		IF NOT EXISTS(SELECT * FROM dbo.IMPORT_FileCatalog WHERE fileBaseName = @FileBaseName)
		BEGIN
			SELECT @Success = 0, @ErrorMessage = ''
			RETURN
		END
		
		-- set the bypass date only if the bypass date is null		
		UPDATE dbo.IMPORT_FileCatalog SET fileLastBypassDate = DATEADD(DAY, 1, fileLastImportFileDate) WHERE fileBaseName = @FileBaseName and fileLastBypassDate IS NULL
		-- increase the last import date with 1 day
		UPDATE dbo.IMPORT_FileCatalog SET fileLastImportFileDate = DATEADD(DAY, 1, fileLastImportFileDate) WHERE fileBaseName = @FileBaseName 

		SELECT @Success = 1, @ErrorMessage = ''

	END TRY

	BEGIN CATCH
		SELECT @Success = 0, @ErrorMessage = ERROR_MESSAGE()
	END CATCH

END
GO
/****** Object:  StoredProcedure [dbo].[spIMPORT_SaveLastFileInfo]    Script Date: 21/04/2023 11:20:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spIMPORT_SaveLastFileInfo]
	 @FileBaseName varchar(100)
	,@RunId INT
	,@FileName varchar(100)
	,@Date date
	,@Success bit OUTPUT
	,@ErrorMessage nvarchar(MAX) OUTPUT
AS

BEGIN
	SET NOCOUNT ON;
	
	BEGIN TRY
	
		IF NOT EXISTS(SELECT * FROM dbo.IMPORT_FileCatalog WHERE fileBaseName = @FileBaseName)
		BEGIN
			SELECT @Success = 0
			RETURN
		END
	
		DECLARE 
			 @lastDate date
		
		UPDATE 
			dbo.IMPORT_FileCatalog
		SET
			fileLastImportFileDate = @Date
		WHERE
			fileBaseName = @FileBaseName

		UPDATE
			dbo.IMPORT_CounterRun
		SET 
			RunId = @RunId

		SELECT @Success = 1, @ErrorMessage = ''

	END TRY

	BEGIN CATCH	

		SELECT @Success = 0, @ErrorMessage = ERROR_MESSAGE()
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[spIMPORT_SyncXmlTableModelWithDb]    Script Date: 21/04/2023 11:20:19 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[spIMPORT_SyncXmlTableModelWithDb]	
	 @SchemaName varchar(20)
	,@TableName varchar(100)
	,@TableModel dbo.utp_IMPORT_TableModel READONLY
	,@FileName varchar(100)
	,@FileDate DateTime
	,@FileBaseName varchar(100)
	,@RunId int
	,@WhichCase varchar(50)
	,@DropIfExists bit = 0
	,@TableExists bit OUTPUT
	,@Success bit OUTPUT
	,@ErrorMessage nvarchar(MAX) OUTPUT

 AS
 
BEGIN
	DECLARE 
		 @sql nvarchar(MAX)
		,@cols nvarchar(MAX)
		,@addTable varchar(30) = 'add_table'
		,@add varchar(30) = 'add_table'

	
	-- This proc checks if the table exists already, 
	-- if not, it create the table with a base model and 
	-- add records into the FileCatalog and TableChanges
	
	BEGIN TRY
		DECLARE @model TABLE(columnName nvarchar(200), columnType nvarchar(30), columnSize int)
	
		SELECT @TableExists = 0, @Success = 0, @ErrorMessage = ''

		-- check for SQL injection
		IF CHARINDEX(';', @SchemaName) > 0 RETURN
		IF CHARINDEX(';', @TableName) > 0 RETURN


		-- check ik the table should be bypassed
		IF EXISTS(SELECT * FROM dbo.IMPORT_BypassObjects as b WHERE b.bypassTable = 1 and b.tableName = @TableName)
		BEGIN
			SELECT @Success = 1, @ErrorMessage = 'Table is bypassed'
		END

		-- transfer the @TableModel to @model so we can remove the bypassed columns and existing columns
		INSERT INTO @model Select * from @TableModel
		-- delete the bypassed columns
		DELETE FROM @model 
		WHERE columnName IN (SELECT b.columnName FROM dbo.IMPORT_BypassObjects as b WHERE b.bypassTable = 0 and b.tableName = @TableName )
		
		
		-- check if the table exists
		IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(@SchemaName + '.' + @TableName) AND type in (N'U')) 
		BEGIN
			-- return that the table exists already
			SELECT @TableExists = 1
			-- drop the table if @DropIfExists = 1
			IF @DropIfExists = 1 
			BEGIN
				SELECT @sql = 'DROP TABLE ' + @SchemaName +  '.' + @TableName
				EXEC sp_executesql @sql
				SELECT @TableExists = 0
			END			
		END

		IF @TableExists = 0
		BEGIN
			-- create the table
			SELECT @sql = 'CREATE TABLE ' + @SchemaName + '.' + @TableName + '(recordId int IDENTITY(1,1) NOT NULL,dins datetime NULL,BRRDDEL bit NULL,fileName varchar(200) NULL,fileDate date NULL,runId int NULL,'
			+ 'CONSTRAINT PK_' + @TableName + ' PRIMARY KEY CLUSTERED (recordId ASC) '
			+ 'WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]) '
			+ 'ON [PRIMARY]'
			EXEC sp_executesql @sql

			-- set default for dins
			SELECT @sql = 'ALTER TABLE ' + @SchemaName + '.' + @TableName + ' ADD  CONSTRAINT DF_' + @TableName + '_dins  DEFAULT (getdate()) FOR dins'
			EXEC sp_executesql @sql
			-- set default for BRRDDEL
			SELECT @sql = 'ALTER TABLE ' + @SchemaName + '.' + @TableName + ' ADD  CONSTRAINT DF_' + @TableName + '_BRRDDEL  DEFAULT ((0)) FOR BRRDDEL'
			EXEC sp_executesql @sql

			-- add the record to the TableChanges
			INSERT INTO dbo.IMPORT_TableChanges(whichCase, tableName, objectType, actionType, actionTypeId, fileName, fileDate, runId)
			SELECT @WhichCase, @TableName, 'table', 'add_table', 1, @FileName, @FileDate, @RunId

			-- create the columns from the model
			SELECT @sql = 'ALTER TABLE ' + @SchemaName + '.' + @TableName + ' ADD ', @cols = ''
			DECLARE @cName nvarchar(300), @cType nvarchar(30)
			DECLARE C CURSOR FOR SELECT columnName, columnType FROM @model
			OPEN C
				FETCH NEXT FROM C INTO @cName, @cType
				WHILE @@FETCH_STATUS = 0
				BEGIN
					-- build the cols string
					SELECT @cols = @cols + '[' + @cName + '] ' + @cType + ' NULL,'

					-- add the info to the tablechanges table
					INSERT INTO dbo.IMPORT_TableChanges(whichCase,tableName, objectType, columnName, actionType, actionTypeId, fileName, fileDate, runId)
					Select @WhichCase, @TableName, 'column', @cName, 'add_column', 3, @FileName, @FileDate, @RunId

					FETCH NEXT FROM C INTO @cName, @cType
				END
			CLOSE C
			DEALLOCATE C
			-- exec the @sql
			IF LEN(@cols) > 0
			BEGIN
				SELECT @sql = @sql + SUBSTRING(@cols, 1, LEN(@cols)-1)
				EXEC sp_executesql @sql
			END
		END
		-- if table exists
		ELSE
		BEGIN
			DECLARE @flds TABLE(fieldName varchar(200), fieldTypeId tinyint, fieldType varchar(40), fieldSize smallint)
			insert into @flds
			SELECT 
				 c.name AS fieldName
				,c.system_type_id as fieldTypeId
				,ft.name as fieldType
				,c.max_length as fieldSize
			FROM 
				sys.columns AS c
				INNER JOIN sys.objects AS o ON c.object_id = o.object_id
				INNER JOIN sys.types as ft on c.system_type_id = ft.system_type_id
			WHERE 
				o.type = 'U' 
				and o.name = @TableName
				and c.name NOT in ('recordId', 'dins', 'BRRDDEL', 'fileName', 'fileDate', 'runId')
				

			-- alter the table, resize the text columns for those who need to be enlarged
			-- ALTER COLUMN statements must be executed one by one
			DECLARE C CURSOR FOR 
				Select md.columnName, md.columnType				
				from 
					@flds as flds
					inner join @model as md on flds.fieldName = md.columnName and (flds.fieldSize < md.columnSize and flds.fieldSize > 0)
				where
					flds.fieldTypeId in (35,99,231,239,167,175)			
			OPEN C
				FETCH NEXT FROM C INTO @cName, @cType
				WHILE @@FETCH_STATUS = 0
				BEGIN
					SELECT @sql = 'ALTER TABLE ' + @SchemaName + '.' + @TableName + ' ALTER COLUMN [' + @cName + '] ' + @cType + ';'
					EXEC sp_executesql @sql
					-- build the cols string
					--SELECT @cols = @cols + '[' + @cName + '] ' + @cType + ' NULL,'					
					FETCH NEXT FROM C INTO @cName, @cType
				END
			CLOSE C
			DEALLOCATE C
			-- exec the @sql


			-- alter the table, add the new columns		
			--reset the @sql
			SELECT @sql = 'ALTER TABLE ' + @SchemaName + '.' + @TableName + ' ADD ', @cols = ''			
			DECLARE C CURSOR FOR 
				Select md.columnName, md.columnType				
				from 
					@flds as flds
					right join @model as md on flds.fieldName = md.columnName
				where 
					flds.fieldName is null		
			OPEN C
				FETCH NEXT FROM C INTO @cName, @cType
				WHILE @@FETCH_STATUS = 0
				BEGIN
					-- build the cols string
					SELECT @cols = @cols + '[' + @cName + '] ' + @cType + ' NULL,'	
					-- add the info to the tablechanges table
					INSERT INTO dbo.IMPORT_TableChanges(whichCase,tableName, objectType, columnName, actionType, actionTypeId, fileName, fileDate, runId)
					Select @WhichCase, @TableName, 'column', @cName, 'add_column', 3, @FileName, @FileDate, @RunId
					FETCH NEXT FROM C INTO @cName, @cType
				END
			CLOSE C
			DEALLOCATE C
			-- exec the @sql
			IF LEN(@cols) > 0
			BEGIN
				SELECT @sql = @sql + SUBSTRING(@cols, 1, LEN(@cols)-1)
				EXEC sp_executesql @sql
			END

			-- save the fields that are no more present in the BIX
			-- be sure to add a remove only once
			INSERT INTO dbo.IMPORT_TableChanges(whichCase,tableName, objectType, columnName, actionType, actionTypeId, fileName, fileDate, runId)
			Select @WhichCase, @TableName, 'column', flds.fieldName, 'remove_column', 4, @FileName, @FileDate, @RunId			
			FROM 
				@flds as flds
				left join @model as md on flds.fieldName = md.columnName
				left join (select * from dbo.IMPORT_TableChanges where actionTypeId = 4 and tableName = @TableName) as rem on flds.fieldName = rem.columnName
			WHERE 
				md.columnName is null
				and rem.tableChangeId is null
				

			-- check if the tableModel for existing fields
			-- ex: check if a DateTime field has been saved as a text field
			INSERT INTO dbo.IMPORT_TableChanges(whichCase,tableName, objectType, columnName, columnTypeTable, columnTypeBixFile, actionType, actionTypeId, fileName, fileDate, runId)
			SELECT @WhichCase, @TableName, 'column', m.columnName, tp.name, m.columnType, 'type_change', 5, @FileName, @FileDate, @RunId
			FROM
				(Select object_id from sys.tables as t0 inner join sys.schemas as s0 on t0.schema_id = s0.schema_id where t0.name = @TableName and s0.name = @SchemaName ) as t
				inner join sys.columns as c on t.object_id = c.object_id				
				inner join sys.types as tp on c.system_type_id = tp.system_type_id
				inner join @TableModel as m on c.name = m.columnName
			WHERE PATINDEX(tp.name + '%', m.columnType) = 0

			-- delete the type changes that shouldn't be registered
			DELETE FROM 
				dbo.IMPORT_TableChanges 
			WHERE 
				actionTypeId = 5 
				and 
				(
					columnTypeTable = 'varchar' and columnTypeBixFile like 'varchar%'
					or columnTypeTable = 'datetime2' and columnTypeBixFile like 'datetime2%'
					or columnTypeTable = 'bit' and columnTypeBixFile = 'bit'
					or columnTypeTable = 'float' and columnTypeBixFile = 'float'
					or columnTypeTable = 'int' and columnTypeBixFile = 'int'
				)

			-- delete the double entries for the field type changes
			-- keep the 1st record where the change appeared
			DELETE 
				tbl 
			FROM 
				(
				select ROW_NUMBER() over (PARTITION BY actionTypeId, tableName, columnName, columnTypeTable ORDER BY fileDate) as idx,tableChangeId from dbo.IMPORT_TableChanges where actionTypeId = 5
				) as tbl
			WHERE 
				idx > 1
		
		END

		SELECT * FROM @model

		SELECT @Success = 1, @ErrorMessage = ''
	END TRY

	BEGIN CATCH	
		SELECT * FROM @model
		SELECT @Success = 0, @ErrorMessage = ERROR_MESSAGE() + ' | @sql = ' + @sql
	END CATCH


END

GO
/****** Object:  StoredProcedure [dbo].[spIMPORT_SyncXmlTableNamesWithDb]    Script Date: 21/04/2023 11:20:19 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[spIMPORT_SyncXmlTableNamesWithDb]	
	 @TableNames dbo.utp_IMPORT_TableList READONLY
	,@WhichCase varchar(100)
	,@FileName varchar(100)
	,@FileDate DateTime2(7)
	,@RunId int
	,@Success bit OUTPUT
	,@ErrorMessage nvarchar(MAX) OUTPUT

 AS
 
BEGIN
	DECLARE 
		 @tableName varchar(100)
		,@actionType varchar(30) = 'remove_table'
		,@objectType varchar(30) = 'table'
		,@actionTypeId int = 2

	-- TODO:
	-- ADD tablePREFIX to the tablelist


	-- This proc checks if the a table is still in the XML, 
	-- if not, it create record in the dbo.IMPORT_TableChanges table
	-- if a table was removed in a previous file but available in this file, it will be logged also

	
	BEGIN TRY

			-- record the tables that have been removed in this XML file
			-- take the bypassed tables into account
			INSERT INTO dbo.IMPORT_TableChanges
			(
			 whichCase
			,tableName
			,objectType
			,actionType
			,actionTypeId
			,fileName
			,fileDate
			,runId
			)
			SELECT 
				 @WhichCase
				,tc.tableName
				,@objectType
				,@actionType
				,@actionTypeId
				,@FileName
				,@FileDate
				,@RunId
			FROM
				(Select tableName from dbo.IMPORT_TableChanges where actionTypeId = 1 and whichCase = @WhichCase) as tc
				LEFT JOIN @TableNames as tn on tc.tableName = tn.tableName
				LEFT JOIN (Select tableName from dbo.IMPORT_BypassObjects where bypassTable = 1) as b on tc.tableName = b.tableName
			WHERE 
				tn.tableName IS NULL
				AND b.tableName IS NULL

				


			-- record the tables that have been previously removed but do appear in this XML now
			-- take the bypassed tables into account
			INSERT INTO dbo.IMPORT_TableChanges
			(
			 whichCase
			,tableName
			,objectType
			,actionType
			,actionTypeId
			,fileName
			,fileDate
			,runId
			)			
			SELECT 
				 @WhichCase
				,tbl.tableName
				,@objectType
				,@actionType
				,@actionTypeId
				,@FileName
				,@FileDate
				,@RunId
			FROM 
				(select tableName, actionTypeId as lastActionTypeId, ROW_NUMBER() OVER (PARTITION BY tableName ORDER BY fileDate desc) as idx from dbo.IMPORT_TableChanges where objectType = 'table' and whichCase = @WhichCase) as tbl
				LEFT JOIN @TableNames as tn on tbl.tableName = tn.tableName
				LEFT JOIN (Select tableName from dbo.IMPORT_BypassObjects where bypassTable = 1) as b on tbl.tableName = b.tableName
			WHERE 
				tbl.idx = 1 
				AND tbl.lastActionTypeId = 2
				AND tn.tableName IS NULL
				AND b.tableName IS NULL
			ORDER BY 
				tbl.tableName


		SELECT @Success = 1, @ErrorMessage = ''
	END TRY

	BEGIN CATCH	

		SELECT @Success = 0, @ErrorMessage = ERROR_MESSAGE()
	END CATCH


END

GO
/****** Object:  StoredProcedure [dbo].[spInsertTableChanges]    Script Date: 21/04/2023 11:20:19 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[spInsertTableChanges]
@whichcase VARCHAR(20),
@category VARCHAR(10),
@description VARCHAR(200)

AS

BEGIN
	SET NOCOUNT ON;

-- START: Log de l'utilisation de la SP
--	EXEC [dbo].[stp_LogSpUsage] @CallerProcId = @@PROCID ;
-- END: Log de l'utilisation de la SP

INSERT INTO [dbo].[TableChanges]
           ([WhichCase]
		   ,[Category]
           ,[Description])
     VALUES
           (@whichcase
		   ,@category
           ,@description)

END
GO
/****** Object:  StoredProcedure [dbo].[spSQLDigger]    Script Date: 21/04/2023 11:20:19 ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[spSQLDigger]
	@SearchText varchar(400)

 AS
 
 BEGIN

	SELECT @SearchText = REPLACE(REPLACE(REPLACE(@SearchText, '[', ''), ']', ''),'''', '');

	DECLARE @tbl table(ObjectName nvarchar(255), ObjectType nvarchar(255), Content nvarchar(MAX), ContentClean nvarchar(MAX))


	insert into @tbl
	SELECT 
			OBJECT_NAME(sm.object_id) AS 'ObjectName'
		,o.type_desc AS 'ObjectType'
		,sm.DEFINITION AS 'Content'
		-- remove carriage returns, [ and ' from conmtent
		,REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(sm.DEFINITION, '[', ''), ']', ''),'''', ''),CHAR(13), ''),CHAR(10), ''),' ','#@'),'@#',''),'#@',' ') as ContentClean
	FROM 
		sys.sql_modules AS sm
		JOIN sys.objects AS o ON sm.object_id = o.object_id	
	UNION	
	SELECT 
			o.name AS 'ObjectName'
		,o.type_desc AS 'ObjectType'
		,c.name AS 'Content'
		,c.name AS ContentClean
	FROM 
		sys.columns AS c
		JOIN sys.objects AS o ON c.object_id = o.object_id
		
	SELECT ObjectName, ObjectType, Content FROM @tbl
	WHERE 
		ContentClean LIKE '%' + @SearchText + '%'
		--OR ObjectName LIKE '%' + @SearchText + '%'
	ORDER by ObjectType, ObjectName


END

GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'type of object being altered: Table or Column' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IMPORT_TableChanges', @level2type=N'COLUMN',@level2name=N'objectType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Type of altering: Added or Removed' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IMPORT_TableChanges', @level2type=N'COLUMN',@level2name=N'actionType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'The file that contained this change' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'IMPORT_TableChanges', @level2type=N'COLUMN',@level2name=N'fileName'
GO
