using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
namespace DGPF.UTILITY
{
    public class ExcelTools
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">传入文件地址</param>
        /// <param name="modePath"模板地址</param>
        /// <param name="result">返回信息</param>
        /// <param name="dt">返回dt数据</param>
        public void GetDataTable(System.IO.Stream reader,string path, string modePath,ref string result,ref DataTable dt) {
            try
            {
                DataTable dtModel;
                string fileFullName = Path.GetFileName(path);
                string fileExt = fileFullName.Substring(fileFullName.IndexOf(".") + 1);
                if (fileExt != "xls" && fileExt != "xlsx")
                {
                    result = "请选择Excel文件类型导入!";
                    return;
                }
                else
                {
                    if (fileExt == "xls")
                    {
                        dt = RenderDataTableFromExcel(System.IO.File.OpenRead(path));
                    }

                    else
                    {
                        dt =RenderDataTableFromExcel2007(reader);
                    }
                    fileExt = modePath.Substring(modePath.LastIndexOf(".") + 1);

                    if (fileExt == "xls")
                    {
                        dtModel = RenderDataTableFromExcel(System.IO.File.OpenRead(modePath));
                    }
                    else
                    {
                        dtModel = RenderDataTableFormExcelHeader2007(modePath);
                    }

                    if (dt.Columns.Count != dtModel.Columns.Count)
                    {
                        result = "抱歉，您选择的导入文件不正确，当前系统检测到您的导入文件的列数与服务器提供的模板列数不相符！请您仔细检查当前导入文件是否正确！";
                        return;
                    }
                    else if (dt.Columns.Count > 0)
                    {
                        int columnNum = 0;
                        columnNum = dt.Columns.Count;
                        string[] strColumns = new string[columnNum];
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            if (dtModel.Columns[i].ColumnName != dt.Columns[i].ColumnName)
                            {
                                result = "抱歉，您选择的导入文件不正确，当前系统检测到您的导入文件中的列名：“" + dtModel.Columns[i].ColumnName + "”，与服务器提供的模板字段不相符！请您仔细检查当前导入文件是否正确！";
                                break;
                            }
                        }
                    }
                    
                    //dtModel.Merge(dt, true, MissingSchemaAction.Ignore);
                }
            }
            catch (Exception ex)
            {
                result=ex.Message;
            }
           
         }



        public static DataTable RenderDataTableFromExcel(Stream excelFileStream)
        {
            using (excelFileStream)
            {
                IWorkbook workbook = new HSSFWorkbook(excelFileStream);

                ISheet sheet = workbook.GetSheetAt(0);//取第一个表 

                DataTable table = new DataTable();

                IRow headerRow = sheet.GetRow(0);//第一行为标题行 
                int cellCount = headerRow.LastCellNum;//LastCellNum = PhysicalNumberOfCells 
                int rowCount = sheet.LastRowNum;//LastRowNum = PhysicalNumberOfRows - 1 

                //handling header. 
                for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                {
                    DataColumn column = new DataColumn(headerRow.GetCell(i).StringCellValue);
                    table.Columns.Add(column);
                }

                for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
                {
                    IRow row = sheet.GetRow(i);
                    if (row == null)
                        break;

                    if (row != null)
                    {
                        if (row.GetCell(0) == null)
                        {
                            break;
                        }
                        if (row.GetCell(0).ToString().Trim() == "")
                        {
                            break;
                        }
                        DataRow dataRow = table.NewRow();
                        for (int j = row.FirstCellNum; j < cellCount; j++)
                        {
                            if (row.GetCell(j) != null)
                                dataRow[j] = row.GetCell(j).ToString();
                        }
                        table.Rows.Add(dataRow);
                    }
                }
                workbook = null;
                sheet = null;
                return table;

            }

        }
        public static DataTable RenderDataTableFromExcel2007(Stream excelFileStream)
        {
            DataTable table = new DataTable();
            try
            {
                using (excelFileStream)
                {
                    IWorkbook workbook = new XSSFWorkbook(excelFileStream);

                    ISheet sheet = workbook.GetSheetAt(0);//取第一个表 

                    IRow headerRow = sheet.GetRow(0);//第一行为标题行 
                    int cellCount = headerRow.LastCellNum;//LastCellNum = PhysicalNumberOfCells 
                    int rowCount = sheet.LastRowNum;//LastRowNum = PhysicalNumberOfRows - 1 

                    //handling header. 
                    for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                    {
                        string columnname = headerRow.GetCell(i).StringCellValue;
                        if (columnname == "")
                            continue;
                        DataColumn column = new DataColumn(columnname);
                        table.Columns.Add(column);
                    }

                    for (int i = (sheet.FirstRowNum + 1); i <= rowCount; i++)
                    {
                        IRow row = sheet.GetRow(i);
                        if (row == null)
                            break;
                        if (row.FirstCellNum < 0)
                        {
                            continue;
                        }
                        else if (row.GetCell(row.FirstCellNum).ToString().Trim() == "")
                        {
                            continue;
                        }

                        DataRow dataRow = table.NewRow();

                        if (row != null)
                        {
                            for (int j = row.FirstCellNum; j < cellCount; j++)
                            {
                                if (row.GetCell(j) != null)
                                {
                                    switch (row.GetCell(j).CellType)
                                    { //空数据类型处理
                                        case CellType.Blank:
                                            dataRow[j] = "";
                                            break;
                                        case CellType.String:
                                            dataRow[j] = row.GetCell(j).StringCellValue;
                                            break;
                                        case CellType.Numeric: //数字类型  
                                            if (HSSFDateUtil.IsCellDateFormatted(row.GetCell(j)))
                                            {
                                                dataRow[j] = row.GetCell(j).DateCellValue;
                                            }
                                            else
                                            {
                                                dataRow[j] = row.GetCell(j).NumericCellValue;
                                            }
                                            break;
                                        case CellType.Formula:
                                            dataRow[j] = row.GetCell(j).NumericCellValue;
                                            break;
                                        default:
                                            dataRow[j] = "";
                                            break;
                                    }
                                }
                            }
                        }

                        table.Rows.Add(dataRow);
                    }
                    workbook = null;
                    sheet = null;
                    return table;

                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return null;
            }
        }
        public static DataTable RenderDataTableFormExcelHeader2007(string filePath)
        {

            DataTable table = new DataTable();
            try
            {
                IWorkbook workbook;
                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    workbook = new XSSFWorkbook(file);
                }

                ISheet sheet = workbook.GetSheetAt(0);//取第一个表 

                IRow headerRow = sheet.GetRow(0);//第一行为标题行 
                int cellCount = headerRow.LastCellNum;//LastCellNum = PhysicalNumberOfCells 
                int rowCount = sheet.LastRowNum;//LastRowNum = PhysicalNumberOfRows - 1 

                for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                {
                    string colName = headerRow.GetCell(i).StringCellValue;
                    if (colName == "")
                        continue;
                    DataColumn column = new DataColumn(colName);
                    table.Columns.Add(column);
                }

                for (int i = 1; i <= 1; i++)
                {
                    IRow row = sheet.GetRow(i);
                    DataRow dataRow = table.NewRow();

                    if (row != null)
                    {
                        for (int j = row.FirstCellNum; j < cellCount; j++)
                        {
                            if (row.GetCell(j) != null)
                                dataRow[j] = "";
                        }
                    }

                    table.Rows.Add(dataRow);
                }

                workbook = null;
                sheet = null;
                return table;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return null;
            }
        }

    }
}
