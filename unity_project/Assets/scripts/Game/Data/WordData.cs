using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WordData {
	private static bool initialized = false;
	private static Dictionary<int, List<WordData>> wordDict = new Dictionary<int, List<WordData>>(100);
	private static WordData[] wordLibrary = new WordData[]{
		new WordData("ant","蚂蚁"),
		new WordData("arm","手臂"),
		new WordData("ask","询问"),
		new WordData("bad","坏的"),
		new WordData("bag","包"),
		new WordData("bee","蜜蜂"),
		new WordData("bet","打赌"),
		new WordData("box","盒子"),
		new WordData("boy","男孩"),
		new WordData("cat","猫"),
		new WordData("cow","奶牛"),
		new WordData("dad","爸爸"),
		new WordData("dog","狗"),
		new WordData("eye","眼睛"),
		new WordData("ear","耳朵"),
		new WordData("fox","狐狸"),
		new WordData("hen","母鸡"),
		new WordData("kid","小孩"),
		new WordData("leg","腿"),
		new WordData("man","男人"),
		new WordData("mom","妈妈"),
		new WordData("pig","猪"),
		new WordData("pen","钢笔"),
		new WordData("red","红"),
		new WordData("son","儿子"),

		new WordData("bear","熊"),
		new WordData("bird","鸟"),
		new WordData("blue","蓝"),
		new WordData("book","书"),
		new WordData("deer","鹿"),
		new WordData("duck","鸭"),
		new WordData("face","脸"),
		new WordData("fish","鱼"),
		new WordData("foot","脚"),
		new WordData("girl","女孩"),
		new WordData("goat","山羊"),
		new WordData("hair","头发"),
		new WordData("head","头"),
		new WordData("hand","手"),
		new WordData("lamb","小羊"),
		new WordData("lion","狮"),
		new WordData("nose","鼻子"),
		new WordData("pink","粉红"),
		new WordData("seal","海豹"),
		new WordData("snow","雪"),
		new WordData("tail","尾巴"),
		new WordData("what","什么"),
		new WordData("word","文字"),
		new WordData("work","工作"),

		new WordData("black","黑"),
		new WordData("brown","棕"),
		new WordData("ruler","尺子"),
		new WordData("mouth","嘴"),
		new WordData("green","绿"),
		new WordData("white","白"),
		new WordData("horse","马"),
		new WordData("snake","蛇"),
		new WordData("mouse","鼠"),
		new WordData("panda","熊猫"),
		new WordData("tiger","老虎"),
		new WordData("zebra","斑马 "),
		new WordData("goose","鹅"),
		new WordData("sheep","绵羊 "),
		new WordData("shark","鲨鱼"),
		new WordData("uncle","叔叔"),
		new WordData("woman","女人"),
		new WordData("queen","女王"),
		new WordData("robot","机器人"),
		new WordData("nurse","护士"),

		new WordData("pencil","铅笔"),
		new WordData("eraser","橡皮"),
		new WordData("crayon","蜡笔"),
		new WordData("finger","手指"),
		new WordData("yellow","黄"),
		new WordData("purple","紫"),
		new WordData("orange","橙"),
		new WordData("rabbit","兔子"),
		new WordData("monkey","猴子"),
		new WordData("turkey","火鸡"),
		new WordData("friend","朋友"),
		new WordData("mother","母亲"),
		new WordData("father","父亲"),
		new WordData("sister","姐妹"),
		new WordData("people","人物"),
		new WordData("doctor","医生"),
		new WordData("driver","司机"),
		new WordData("farmer","农民 "),
		new WordData("singer","歌唱家"),

		new WordData("giraffe","长颈鹿"),
		new WordData("brother","兄弟"),
		new WordData("parents","父母"),
		new WordData("visitor","参观者"),
		new WordData("tourist","旅行者"),
		new WordData("teacher","教师"),
		new WordData("student","学生"),
		new WordData("actress","女演员"),
		new WordData("cleaner","清洁工"),
		new WordData("biscuit","饼干"),

		new WordData("notebook","笔记本"),
		new WordData("magazine","杂志"),
		new WordData("elephant","大象"),
		new WordData("kangaroo","袋鼠"),
		new WordData("engineer","工程师"),
		new WordData("sandwich","三明治"),
		new WordData("eggplant","茄子"),
		new WordData("cucumber","黄瓜"),
		new WordData("raincoat","雨衣"),
		new WordData("computer","计算机"),

		new WordData("newspaper","报纸"),
		new WordData("schoolbag","书包"),
		new WordData("sharpener","卷笔刀"),
		new WordData("newspaper","报纸"),
		new WordData("classmate","同学"),
		new WordData("neighbour","邻居"),
		new WordData("principal","校长 "),
		new WordData("policeman","（男）警察"),
		new WordData("assistant","售货员"),
		new WordData("breakfast","早餐"),

		new WordData("maths book","数学书"),
		new WordData("dictionary","词典"),
		new WordData("watermelon","西瓜"),
		new WordData("strawberry","草莓"),
		new WordData("sunglasses","太阳镜"),
		new WordData("chopsticks","筷子"),
		new WordData("toothbrush","牙刷"),
		new WordData("playground","操场"),
		new WordData("salesperson","销售员"),
		new WordData("supermarket","超市"),
		new WordData("consequence","结果，成果"),
		new WordData("acknowledge","承认，鸣谢"),
		new WordData("abomination","憎恶，憎恨"),
		new WordData("underground","地下的，秘密的"),
		new WordData("agriculture","农业"),
		new WordData("administrate","管理"),
		new WordData("neighborhood","附近"),
		new WordData("congratulate","祝贺"),
		new WordData("headquarters","总部，司令部"),
		new WordData("organisation","组织")

	};

	public int length;
	public string word;
	public string translation;

	public WordData(string word, string translation)
	{
		this.length = word.Length;
		this.word = word;
		this.translation = translation;
	}

	public static WordData GetRandomWord(int length)
	{
		if (initialized == false)
		{
			Initialize();
			initialized = true;
		}
		List<WordData> wordArray = null;
		if (wordDict.TryGetValue(length, out wordArray))
		{
			int randomIndex = Random.Range(0, wordArray.Count);
			return wordArray[randomIndex];
		}
		else
		{
			return null;
		}
	}

	private static void Initialize()
	{
		foreach(WordData wordData in wordLibrary)
		{
			if(wordDict.ContainsKey(wordData.length))
			{
				wordDict[wordData.length].Add(wordData);
			}
			else
			{
				wordDict.Add(wordData.length, new List<WordData>(100));
			}
		}
	}
}
