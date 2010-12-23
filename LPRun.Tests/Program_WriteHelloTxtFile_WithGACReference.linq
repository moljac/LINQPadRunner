<Query Kind="Program">
  <GACReference>System.Configuration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a</GACReference>
  <Namespace>System.Net</Namespace>
  <Namespace>System.Data</Namespace>
</Query>

void Main()
{
	var x = typeof(System.Configuration.ConfigurationException);
	File.WriteAllText("hello.txt", "world");
}

// Define other methods and classes here