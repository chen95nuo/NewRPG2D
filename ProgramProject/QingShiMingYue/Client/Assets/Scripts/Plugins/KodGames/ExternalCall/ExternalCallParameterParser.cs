using System;

namespace KodGames.ExternalCall
{
	public class ExternalCallParameterParser
	{
		private const char ParamSeparater = '#';

		public ExternalCallParameterParser(string paramString)
		{
			if (paramString != null)
			{
				parameters = paramString.Split('#');
			}

			if (parameters == null)
				parameters = new string[0];

			paramIdx = 0;
		}

		public bool Parse(out bool value)
		{
			double parseDouble;
			if (paramIdx < parameters.Length && double.TryParse(parameters[paramIdx], out parseDouble))
			{
				value = parseDouble != 0;

				++paramIdx;
				return true;
			}

			value = false;
			return false;
		}

		public bool Parse(out int value)
		{
			double parseDouble;
			if (paramIdx < parameters.Length && double.TryParse(parameters[paramIdx], out parseDouble))
			{
				value = (int)parseDouble;

				++paramIdx;
				return true;
			}

			value = 0;
			return false;
		}

		public bool Parse(out uint value)
		{
			double parseDouble;
			if (paramIdx < parameters.Length && double.TryParse(parameters[paramIdx], out parseDouble))
			{
				value = (uint)parseDouble;

				++paramIdx;
				return true;
			}

			value = 0;
			return false;
		}

		public bool Parse(out long value)
		{
			long parseLong;
			if (paramIdx < parameters.Length && long.TryParse(parameters[paramIdx], out parseLong))
			{
				value = parseLong;

				++paramIdx;
				return true;
			}

			value = 0;
			return false;
		}

		public bool Parse(out string value)
		{
			if (paramIdx < parameters.Length)
			{
				value = parameters[paramIdx];

				++paramIdx;
				return true;
			}

			value = "";
			return false;
		}

		private int paramIdx;
		private string[] parameters;
	}
}
