using System;
using ClientServerCommon;

public class MathParser : IMathParser
{
	public MathParser(string expression)
	{
		parser = new Bestcode.MathParser.MathParser();
		SetExpression(expression);
	}

	public void SetExpression(string expression)
	{
		parser.Expression = expression;
	}

	public string GetExpression()
	{
		return parser.Expression;
	}
	
	public  void SetVariable(string name, float value)
	{
		parser.SetVariable(name, value, null);
	}

	public void RemoveAllVariables()
	{
		parser.DeleteAllVars();
	}

	public double Evaluate()
	{
		return parser.Evaluate().ToDouble(null);
	}

	private Bestcode.MathParser.MathParser parser;
}

public class MathParserFactory : IMathParserFactory
{
	public IMathParser CreateMathParser(string expression)
	{
		return new MathParser(expression);
	}
}

