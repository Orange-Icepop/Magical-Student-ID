#include <bits/stdc++.h>
using namespace std;

int main()
{
	string choice1,choice2;
	bool flag = true;
	int nmax;
	int nmin;
	srand((unsigned)time(NULL));
	cout << "欢迎使用魔法学号命令行版v1.0！" << endl;
	cout << "作者：Bilibili@TNTNTBTT" << endl;
	for (;true;)
	{
		flag = true;
		cout << "请输入最大号码" << endl;
		cin >> nmax;
		cout << "请输入最小号码" << endl;
		cin >> nmin;
		for ( ;flag == true ;)
		{
			cout << "是否开始随机？[y/n]" << endl;
			cin >> choice1;
			if (choice1 == "y")
				cout << "随机结果：" << rand() % nmax + nmin << endl;
			else if (choice1 == "n")
			{
				for (;flag == true; )
				{
					cout << "是否退出魔法学号命令行版？[y/n]" << endl;
					cin >> choice2;
					if (choice2 == "y")
						return 0;
					else if (choice2 == "n")
						flag = false;
					else {
						cout << "非法字符" << endl;
						system("pause");
					}
				}
			}
			else {
				cout << "非法字符" << endl;
				system("pause");
			}
		}
	}
}