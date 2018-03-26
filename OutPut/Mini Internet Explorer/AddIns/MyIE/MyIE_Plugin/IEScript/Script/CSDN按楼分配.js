function GetPointInput(reply)
{
	var inputs = reply.getElementsByTagName("input");
	for (var i = 0; i < inputs.length; i++)
	{
		if (inputs[i].name == "tb_score" && inputs[i].className == "normal")
		{
			return inputs[i];
		}
	}
}

function GetTopicPoint(topic)
{
	var vars = topic.getElementsByTagName("var");
	for (var i = 0; i < vars.length; i++)
	{
		if (vars[i].id == "point")
		{
			return vars[i].innerText;
		}
	}	
}

var tables = document.getElementsByTagName("table");
var replys = new Array();
var pointCounter = 0; // 得分楼数
var topicPoint = -1; // 本帖总分

for (var i = 0; i < tables.length; i++)
{
	if (tables[i].className == "item topic")
	{
		topicPoint = GetTopicPoint(tables[i]);
	}
	if (tables[i].className == "item reply")
	{
		if (GetPointInput(tables[i]))
			pointCounter++;
	}
}
if (topicPoint > 0 && pointCounter > 0) 
{
	var average = Math.floor(topicPoint / pointCounter);
	var spare = topicPoint % pointCounter;

	for (var i = 0; i < tables.length; i++)
	{
		if (tables[i].className == "item reply")
		{
			var input = GetPointInput(tables[i]);
			if (input)
			{
				if (spare > 0)
				{
					input.value = average + 1;
					spare--;
				} else input.value = average;
			}
		}
	}
}
else 
{
	alert("确认是在结贴页面中？");
}