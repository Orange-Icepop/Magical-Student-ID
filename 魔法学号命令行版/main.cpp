#include <bits/stdc++.h>
using namespace std;

int main()
{
	string choice1,choice2;
	bool flag = true;
	int nmax;
	int nmin;
	srand((unsigned)time(NULL));
	cout << "��ӭʹ��ħ��ѧ�������а�v1.0��" << endl;
	cout << "���ߣ�Bilibili@TNTNTBTT" << endl;
	for (;true;)
	{
		flag = true;
		cout << "������������" << endl;
		cin >> nmax;
		cout << "��������С����" << endl;
		cin >> nmin;
		for ( ;flag == true ;)
		{
			cout << "�Ƿ�ʼ�����[y/n]" << endl;
			cin >> choice1;
			if (choice1 == "y")
				cout << "��������" << rand() % nmax + nmin << endl;
			else if (choice1 == "n")
			{
				for (;flag == true; )
				{
					cout << "�Ƿ��˳�ħ��ѧ�������а棿[y/n]" << endl;
					cin >> choice2;
					if (choice2 == "y")
						return 0;
					else if (choice2 == "n")
						flag = false;
					else {
						cout << "�Ƿ��ַ�" << endl;
						system("pause");
					}
				}
			}
			else {
				cout << "�Ƿ��ַ�" << endl;
				system("pause");
			}
		}
	}
}