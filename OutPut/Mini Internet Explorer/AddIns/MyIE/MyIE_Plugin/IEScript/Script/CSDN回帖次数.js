function ReplyCounter(name)
{
	this.name = name;
	this.count = 1;
	return this;
}

function ReplyName(reply)
{
	var vars = reply.getElementsByTagName ("var");
	for (var i = 0; i < vars.length; i++)
	{
		if (vars[i].id == "topicUserName")
			return vars[i].innerText;
	}
}

function ReplyIndex(arr, name)
{
	for (var i = 0; i < arr.length; i++)
	{
		if (arr[i].name == name)
			return i;
	}
	return -1;
}

var tables = document.getElementsByTagName("table");

var replys = new Array();

for (var i = 0; i < tables.length; i++)
{
	if (tables[i].className == "item reply")
	{
		var name = ReplyName(tables[i]);
		var index = ReplyIndex(replys, name);
		if (index < 0)
			replys.push(new ReplyCounter(name));
		else replys[index].count++;
	}
}

function showReport()
{
	var msg = "共有" + replys.length + "人回帖,\n以下是回帖计数:\n"
	for (var i = 0; i < replys.length; i++)
	{
		msg += replys[i].name + ":" + replys[i].count + "\n";
	}
	alert(msg);
}

showReport();