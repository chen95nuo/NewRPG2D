using UnityEngine;
using System.Collections;

public interface PropertyReader{
	void addData();
	void resetData();
	void parse(string[] ss);
}
