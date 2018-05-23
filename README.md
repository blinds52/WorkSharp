### WorkSharp

WorkSharp is a task based workflow composition tool. Its main goal is to allow defining complex async process flows via configuration. 
This is done by creating the building blocks in C# and defining the workflow in JSON utilzing CSharp Script to glue to pieces together.

### Example workflow definitions

```json
{
  "_type": "Sequence",
  "items": [
    { "_type": "Assign", "name": "Scope.Url", "expression": "\"https://jsonplaceholder.typicode.com/posts\"" },
    {
      "_type": "HttpGet",
      "_resultTo": "Scope.ContentList",
      "url": "Scope.Url"
    },
    {
      "_type": "Assign",
      "name": "Scope.SelectedContent",
      "expression": "Scope.ContentList[0]"
    },
    {
      "_type": "Delay",
      "_resultTo":  "Scope.DelayTime",
      "duration": "1000 * 2 + 42"
    },
    {
      "_type": "ConsoleWrite",
      "message": "$\"We were also Delayed {Scope.DelayTime} ms before we could see this -> {Scope.SelectedContent.body}\""
    }
  ]
}
```

### How to use

```C#
	var jsonText = File.ReadAllText(Directory.GetCurrentDirectory() + "\\wf.json");
	ExpandoObject json = JsonConvert.DeserializeObject<ExpandoObject>(jsonText);
	var ws = new WorkSharp.WorkSharp();
	var wf = ws.CreateFromJSON(json);
	var r = await wf.Invoke(((dynamic)new ExpandoObject()));
```