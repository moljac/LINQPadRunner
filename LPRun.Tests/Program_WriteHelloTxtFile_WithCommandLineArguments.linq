<Query Kind="Program" />

void Main()
{
	//use Main() for testing
	Main("hello.txt", "world"); 
}

//this is used as the entry point
void Main(params string[] args)
{
	File.WriteAllText(args[0], args[1]);
}
