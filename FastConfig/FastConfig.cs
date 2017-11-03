using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Configuration;
using System.Globalization;
using System.Reflection;

namespace FastConfigLibrary
{
	public partial class FastConfig
	{
		private const int BOOL = 3, DOUBLE = 2, LONG = 1, STRING = 0, UNSUPPORTED_TYPE = -1;
		public string configPath { get; }

		private const string WRONG_TYPE_EXCEPTION = @"Variable ({0}) has wrong type. Waiting ({1}) type. Error in line {2}.",
		WRONG_FILE_FORMAT_EXCEPTION = @"Wrong file format. Error in line {0}.",
		VARIABLE_NOT_EXIST_EXCEPTION = @"Variable ({0}) doesn't exist. Error in line {1}.",
		UNDEFINED_EXCEPTION = @"Undefined Exception. {0}";

		public FastConfig(string configPath)
		{
			this.configPath = configPath;
			ParseConfigFile();
		}

		private void ParseConfigFile()
		{
			string text = normalize(File.ReadAllText(configPath));

			int currentParseLine = 0;

			string[] sections = Regex.Split(text, "\\[.+\\]");

			for (int i = 0; i < sections.Length; i++)
			{
				if (sections[i].Equals(""))
					continue;
				currentParseLine++;
				string[] sectionValues = sections[i].Split('\n');
				for (int j = 0; j < sectionValues.Length; j++)
				{
					if (sectionValues[j].Trim(' ').Equals(""))
						continue;
					currentParseLine++;
					int index;
					string name, value;
					try
					{
						index = sectionValues[j].IndexOf('=', 0);
						name = sectionValues[j].Substring(0, index);
						value = sectionValues[j].Substring(index + 1);
					}
					catch
					{
						throw new Exception(String.Format(WRONG_FILE_FORMAT_EXCEPTION, currentParseLine.ToString()));
					}
					name = name.Trim(' ');
					value = value.Trim(' ');

					int type = getValueType(name + "=" + value);

					PropertyInfo getVariable = typeof(FastConfig).GetProperty(name);
					if (getVariable == null)
						throw new Exception(String.Format(VARIABLE_NOT_EXIST_EXCEPTION, name, currentParseLine));

					if (type == UNSUPPORTED_TYPE)
					{
						throw new Exception(String.Format(WRONG_FILE_FORMAT_EXCEPTION, currentParseLine.ToString()));
					}
					try
					{
						if (type == DOUBLE)
						{
							getVariable.SetValue(this, Convert.ToDouble(value.Replace(',', '.'), CultureInfo.GetCultureInfo("en-US")), null);

						}
						if (type == LONG)
						{
							getVariable.SetValue(this, Convert.ToInt64(value), null);

						}
						if (type == STRING)
						{
							getVariable.SetValue(this, value.Substring(1, value.Length - 2), null);

						}
						if (type == BOOL)
						{
							getVariable.SetValue(this, Convert.ToBoolean(value), null);

						}
					}
					catch(FormatException e)
					{
						throw new Exception(String.Format(WRONG_TYPE_EXCEPTION, name, getVariable.PropertyType.Name, currentParseLine));
					}
					catch(ArgumentException e)
					{
						throw new Exception(String.Format(UNDEFINED_EXCEPTION, "Try check variable write permission. ") +e.ToString());
					}
				}
			}
		}

		/**
		 * 
		 * 0 - is a string
		 * 1 - is a integer
		 * 2 - is a double
		 * 3 - is a bool
		 * 
		 * */
		private int getValueType(string val)
		{
			// name="value"
			// name=-53
			// name=-45.493 OR name=-45,493 OR name=-45. OR name=-45
			string[] patterns = { @"^[A-Za-z0-9]+={1}"".*""$", @"^[A-Za-z0-9]+={1}[-]{0,1}[0-9]+$", @"^[A-Za-z0-9]+={1}[-]{0,1}[0-9]+[\.,]{0,1}[0-9]*$", "^[A-Za-z0-9]+={1}([Tt][Rr][Uu][Ee]|[Ff][Aa][Ll][Ss][Ee])$" };

			for (int i = 0; i < patterns.Length; i++)
				if (Regex.IsMatch(val, patterns[i]))
					return i;
			return -1;
		}

		private string normalize(string value)
		{
			return value.Replace("\r", "");
		}
	}
}