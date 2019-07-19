DECLARE @MyValue NVarChar(4000) = '1';

SELECT S.name SchemaName, T.name TableName
INTO #T
FROM sys.schemas S INNER JOIN
     sys.tables T ON S.schema_id = T.schema_id;

WHILE (EXISTS (SELECT * FROM #T)) BEGIN
  DECLARE @SQL NVarChar(4000) = 'SELECT * FROM $$TableName WHERE (0 = 1) ';
  DECLARE @TableName NVarChar(1000) = (
    SELECT TOP 1 SchemaName + '.' + TableName FROM #T
  );
  SELECT @SQL = REPLACE(@SQL, '$$TableName', @TableName);

  DECLARE @Cols NVarChar(4000) = '';

  SELECT
    @Cols = COALESCE(@Cols + 'OR CONVERT(NVarChar(4000), ', '') + C.name + ') = CONVERT(NVarChar(4000), ''$$MyValue'') '
  FROM sys.columns C
  WHERE C.object_id = OBJECT_ID(@TableName);

  SELECT @Cols = REPLACE(@Cols, '$$MyValue', @MyValue);
  SELECT @SQL = @SQL + @Cols;

  EXECUTE(@SQL);

  DELETE FROM #T
  WHERE SchemaName + '.' + TableName = @TableName;
END;

DROP TABLE #T;
