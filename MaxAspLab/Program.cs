using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MaxAspLab
{
    //���������� ������� ������
    public class Tree_node
    {
        public Tree_node(char data)
        {
            this.data = data;
        }
        public char data { get; set; }
        public Tree_node Left { get; set; }
        public Tree_node Right { get; set; }

        public void Insert(Tree_node node)
        {
            if (node.data < data)
            {
                if (Left == null)
                    Left = node;
                else
                    Left.Insert(node);
            }
            else
            {
                if (Right == null)
                    Right = node;
                else
                    Right.Insert(node);
            }
        }

        public string Tostring(List<char> list = null)
        {
            if (list == null)
                list = new List<char>();
            if (Left != null)
                Left.Tostring(list);

            list.Add(data);

            if (Right != null)
                Right.Tostring(list);
            //return list.ToArray();

            string a = new string(list.ToArray());
            return a;
        }


    }
    public class Program
    {
        static WebOut webOut = new WebOut();
        //����� ��������� �����
        async static Task HttpRand( string str, FileJson data)
        {
            int length = str.Length - 1;
            try
            {
                using (var client = new HttpClient())
                {
                    string url = string.Format("{1}?min=0&max={0}&count=1", length, data.RandomApi);
                    using HttpResponseMessage response = await client.GetAsync(url);
                    string content = await response.Content.ReadAsStringAsync();

                    webOut.rand = content;
                    content = content.Replace("[", "").Replace("]", "");
                    int rnd = Convert.ToInt32(content);
                    webOut.shortst = str.Remove(rnd, 1);
                }
            }
            catch
            {
                Random rnd = new Random();
                int value = rnd.Next(0, length);
                webOut.rand = Convert.ToString(value);
                webOut.shortst = str.Remove(value, 1);


            }
        }
        //���� ������ �������
        static void glas(string output)
        {
            int flag = 0;
            int id1 = 0;
            int id2 = 0;

            for (int i = 0; i < output.Length; i++)
            {
                if ("aeiouy".Contains(output[i]) && flag == 0)
                {
                    flag = 1;
                    id1 = i;
                }
                else if ("aeiouy".Contains(output[i]))
                {
                    flag = 2;
                    id2 = i;
                }

            }
            if (flag == 2)
                webOut.st2 = output.Substring(id1, ++id2);
                //Console.WriteLine(output.Substring(id1, ++id2));
        }
        //������� ����������
        static string QuickSort(char[] str, int minID, int maxID)
        {
            if (minID >= maxID)
                return new string(str);
            int pivotID = GetPivot(str, minID, maxID);
            QuickSort(str, minID, pivotID - 1);
            QuickSort(str, pivotID + 1, maxID);
            string a = new string(str);
            return a;
        }

        static int GetPivot(char[] str, int minID, int maxID)
        {
            int pivot = minID - 1;
            for (int i = minID; i <= maxID; i++)
            {
                if (str[i] < str[maxID])
                {
                    pivot++;
                    Swap(ref str[pivot], ref str[i]);
                }
            }

            pivot++;
            Swap(ref str[pivot], ref str[maxID]);

            return pivot;
        }

        static void Swap(ref char left, ref char right)
        {
            char temp = left;

            left = right;

            right = temp;
        }

        static string TreeSort(char[] str)
        {

            var tree = new Tree_node(str[0]);
            for (int i = 1; i < str.Length; i++)
                tree.Insert(new Tree_node(str[i]));

            return tree.Tostring();
        }

        static void let(string str)
        {
            var letters = new Dictionary<char, int>();
            foreach (char c in str)
            {
                if (letters.ContainsKey(c))
                    letters[c] = ++letters[c];
                else
                    letters.Add(c, 1);
            }
            webOut.letters = letters;
        }


        static async void Base(HttpContext context, string a, string sw)
        {
            try
            {
                //lab1 Smolnikov
                char[] str = new char[a.Length];
                for (int i = 0; i < a.Length; i++)
                    str[i] = a[i];
                string filepath = @"appsettings.json";
                var json = File.ReadAllText(filepath);
                var data = JsonSerializer.Deserialize<FileJson>(json);
                string output = "";
                string exept = "";
                //���� ������ � ����������� for
                //�������� �� ������������ ����������� ������
                for (int i = 0; i < a.Length; i++)
                {
                    if (a[i] < 97 || a[i] > 122)
                        exept = exept + a[i];
                }
                if (String.IsNullOrEmpty(exept))
                {
                    string[] blacklist = data.Settings.Blacklist;
                    bool flag = false;
                    for (int i = 0; i < blacklist.Length; i++)
                    {
                        if (a.Contains(blacklist[i]))
                            flag = true;
                    }
                    if (!flag)
                    {

                        if (a.Length % 2 == 0)
                        {
                            string b = a.Substring(0, a.Length / 2);
                            char[] bc = b.ToCharArray();
                            Array.Reverse(bc);
                            a = a.Substring(a.Length / 2);
                            char[] ac = a.ToCharArray();
                            Array.Reverse(ac);
                            output = String.Concat<char>(bc) + String.Concat<char>(ac);
                            webOut.st1 = output;
                            //Console.WriteLine(output);
                            let(output);


                            glas(output);
                        }
                        else
                        {
                            char[] b = a.ToCharArray();
                            Array.Reverse(b);
                            output = String.Concat<char>(b) + a;
                            webOut.st1 = output;

                            let(output);
                            glas(output);

                        }






                        switch (sw)
                        {
                            case "1":
                                webOut.sortST = QuickSort(str, 0, str.Length - 1);
                                break;
                            case "2":
                                webOut.sortST = TreeSort(str);
                                break;

                        }

                        HttpRand(output, data);

                        await context.Response.WriteAsJsonAsync(webOut);
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                        await context.Response.WriteAsync($"not walid");
                    }
                }
                else
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsync($"not walid {exept}");

                }
            }

            catch (Exception ex) { }
        }

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            var app = builder.Build();

            //------------------------------------------------------------------------------
            //swagger �� ������� ���������, � ��������� ���� ���������� ��� ���������
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.MapGet("/", async (context) =>
            {
                string a = context.Request.Query["string"];//querry �������� ������ �� ����
                string choice = context.Request.Query["choice"];//querry �������� ����� ����������: 1-������� 2 -�������


                Base(context, a, choice);
            });



            app.Run();
            Console.WriteLine();
            Console.WriteLine();



        
        }
    }
}
