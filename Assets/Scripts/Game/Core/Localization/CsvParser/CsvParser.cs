using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

#pragma warning disable CS1066

namespace Game.Core.Localization
{
    public class CsvParser : ICsvParser
    {
        Dictionary<string, List<string>> ICsvParser.LoadFromPath(string path, Delimiter delimiter = Delimiter.Auto, Encoding encoding = null)
		{
			encoding ??= Encoding.UTF8;

			if (delimiter == Delimiter.Auto)
			{
				delimiter = EstimateDelimiter(path);
			}

			var data = File.ReadAllText(path, encoding);

			return ((ICsvParser)this).Parse(data, delimiter);
		}

		async Task<Dictionary<string, List<string>>> ICsvParser.LoadFromPathAsync(string path, 
		                                                                          Delimiter delimiter = Delimiter.Auto, Encoding encoding = null)
		{
			encoding ??= Encoding.UTF8;

			if (delimiter == Delimiter.Auto)
			{
				delimiter = EstimateDelimiter(path);
			}

			using var reader = new StreamReader(path, encoding);

			var data = await reader.ReadToEndAsync();

			return ((ICsvParser)this).Parse(data, delimiter);
		}

		Dictionary<string, List<string>> ICsvParser.Parse(string data, Delimiter delimiter)
		{
			ConvertToCrlf(ref data);

			var sheet = new Dictionary<string, List<string>>();
			var row = new List<string>();
			var cell = new StringBuilder();
			var insideQuoteCell = false;
			var start = 0;

			var delimiterSpan = delimiter.ToChar().ToString().AsSpan();
			var crlfSpan = "\r\n".AsSpan();
			var oneDoubleQuotSpan = "\"".AsSpan();
			var twoDoubleQuotSpan = "\"\"".AsSpan();

			while (start < data.Length)
			{
				var length = start <= data.Length - 2 ? 2 : 1;
				var span = data.AsSpan(start, length);

				if (span.StartsWith(delimiterSpan))
				{
					if (insideQuoteCell)
					{
						cell.Append(delimiter.ToChar());
					}
					else
					{
						AddCell(row, cell);
					}

					start += 1;
				}
				else if (span.StartsWith(crlfSpan))
				{
					if (insideQuoteCell)
					{
						cell.Append("\r\n");
					}
					else
					{
						AddCell(row, cell);
						AddRow(sheet, ref row);
					}

					start += 2;
				}
				else if (span.StartsWith(twoDoubleQuotSpan))
				{
					cell.Append("\"");
					start += 2;
				}
				else if (span.StartsWith(oneDoubleQuotSpan))
				{
					insideQuoteCell = !insideQuoteCell;
					start += 1;
				}
				else
				{
					cell.Append(span[0]);
					start += 1;
				}
			}

			if (row.Count > 0)
			{
				AddCell(row, cell);
				AddRow(sheet, ref row);
			}

			return sheet;
		}

		private static Delimiter EstimateDelimiter(string path)
		{
			var extension = Path.GetExtension(path);

			if (extension.Equals(".csv", StringComparison.OrdinalIgnoreCase))
			{
				return Delimiter.Comma;
			}

			if (extension.Equals(".tsv", StringComparison.OrdinalIgnoreCase))
			{
				return Delimiter.Tab;
			}

			throw new Exception($"Delimiter estimation failed. Unknown Extension: {extension}");
		}
		
		private static void AddCell(ICollection<string> row, StringBuilder cell)
		{
			row.Add(cell.ToString());
			cell.Length = 0;
		}

		private static void AddRow(Dictionary<string, List<string>> sheet, ref List<string> row)
		{
			var code = row[0];
			row.RemoveAt(0);

			if (sheet.ContainsKey(code) == false)
			{
				sheet.Add(code, row);
			}
			
			row = new List<string>();
		}

		private static void ConvertToCrlf(ref string data)
		{
			data = Regex.Replace(data, @"\r\n|\r|\n", "\r\n");
		}
    }
}