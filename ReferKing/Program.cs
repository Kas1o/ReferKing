﻿// 读取书籍
using System.Text;

if (!Path.Exists("./books"))
{
    Console.WriteLine("缺少./books 文件夹，请在其中放置书籍txt");
    Console.Read();
    return;
}

List<(string bookName, string content)> books = new();
var bookpath = Directory.GetFiles("./books");
foreach(var path in bookpath)
{
    var bookname = Path.GetFileNameWithoutExtension(path);
    var content = "";
    using (StreamReader sr = new(path))
    {
        content = sr.ReadToEnd();
        sr.Close();
    }
    books.Add((bookname, content));
}
if(books.Count == 0)
{
    Console.WriteLine("./books 文件夹中缺失书籍，请在其中放置书籍txt");
    Console.Read();
    return;
}


// 获取用户输入
retry:
var inputString = Console.ReadLine();
if (inputString == "" || inputString == null) goto retry;

Console.WriteLine("# 这行字很有文化底蕴！");

string processString = (string)inputString.Clone();
List<(string content,string bookname, int idx)> refers = new();

while (true)
{
    var results = GetLongest(processString,books);
    processString =  processString.Remove(0,results.matched.Length);
    refers.Add((results.matched, results.bookname, results.idx));

    if (processString.Length == 0)
    {
        break;
    }
}

string[] quotePhrases = { "摘抄自", "节选自", "择萃于", "选自", "引自" };
var random = new Random();
foreach (var item in refers)
{
    string quotePhrase = quotePhrases[random.Next(quotePhrases.Length)];
    Console.WriteLine($"### “{item.content}” {quotePhrase}：{item.bookname}的第{item.idx}字");
    foreach(var book in books)
    {
        if(book.bookName == item.bookname)
        {
            var near =GetNearbyChars(book.content, item.idx);
            near = EncapsulateString(near, item.content);
            near = "> \"" + near + "\"";
            Console.WriteLine(near);
        }
    }
}

Console.ReadLine();
(string matched, int idx, string bookname) GetLongest(string input, List< (string bookName, string content) > books){
    int targetIdx = 0;
    string currentCompare = "";

    int idx = 0;
    string? bookname = null;
    while (true)
    {
        currentCompare += input[targetIdx++];

        bool isMatched = false;
        books.Shuffle();
        foreach(var book in books)
        {
            if(book.content.Contains(currentCompare))
            {
                idx =  book.content.LastIndexOf(currentCompare);
                bookname = book.bookName;
                isMatched = true;
                break;
            }
        }

        if (!isMatched)//没找到，使用上一个结果
        {
            if(bookname == null)//连上一个结果都没有。直接表明原创。
                return (currentCompare,int.MaxValue,"原创");
            return (currentCompare.Remove(currentCompare.Length - 1), idx, bookname);
        }

        if(input.Length == targetIdx)//匹配完毕
        {
            return (input, idx, (string)bookname);
        }
    }
}
static string GetNearbyChars(string x, int index)
{
    string beforeIndex = "";
    string afterIndex = "";
    for(int i = 1; i < 10; i++)
    {
        try
        {
            var tar = x[index+i];
            if (tar == '\n') break;
            afterIndex += tar;
        }
        catch
        {
            break;
        }
    }
    for (int i = 1; i < 10; i++)
    {
        try
        {
            var tar = x[index - i];
            if (tar == '\n') break;
            beforeIndex += tar;
        }
        catch
        {
            break;
        }
    }
    beforeIndex = new string(beforeIndex.Reverse().ToArray());

    return beforeIndex + x[index] + afterIndex;
}
static string EncapsulateString(string b, string a)
{
    int index = b.IndexOf(a);
    StringBuilder sb = new StringBuilder(b);

    while (index != -1)
    {
        sb.Insert(index, "**");
        sb.Insert(index + a.Length + 2, "**");

        int nextIndex = index + a.Length + 4;
        if (nextIndex >= b.Length)
            break;

        index = b.IndexOf(a, nextIndex);
    }

    return sb.ToString();
}