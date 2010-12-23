<Query Kind="Program">
  <Namespace>System.Net</Namespace>
  <Namespace>System.Data</Namespace>
</Query>

void Main()
{
	var wc = new WebClient();
	File.WriteAllText("hello.txt", "world");
}

// Define other methods and classes here