#include <iostream>
#include <math.h>

int main()
{
	int height = 10;

	// 역피라미드
	std::cout << "역피라미드\r\n";
	for (int i = 0; i < height; ++i)
	{
		for (int j = 0; j < i; ++j)
		{
			std::cout << " ";
		}

		for (int j = 0; j < height * 2 - i * 2 - 1; ++j)
		{
			std::cout << "*";
		}

		std::cout << "\r\n";
	}


	std::cout << "\r\n";
	std::cout << "\r\n";

	// 속이 빈 마름모
	std::cout << "속이 빈 마름모\r\n";
	for (int i = 0; i < height; ++i)
	{
		for (int j = 0; j < height - i - 1; ++j)
		{
			std::cout << " ";
		}
		
		if (i != height - 1)
		{
			for (int j = 0; j <= i * 2; ++j)
			{
				if (j == 0 || j == i * 2)
				{
					std::cout << "*";
				}
				else
				{
					std::cout << " ";
				}
			}

			std::cout << "\r\n";
		}
	}

	for (int i = 0; i < height; ++i)
	{
		for (int j = 0; j < i; ++j)
		{
			std::cout << " ";
		}

		for (int j = 0; j < height * 2 - i * 2 - 1; ++j)
		{
			if (j == 0 || j == height * 2 - i * 2 - 2)
			{
				std::cout << "*";
			}
			else
			{
				std::cout << " ";
			}
		}

		std::cout << "\r\n";

	}
	
	std::cout << "\r\n";

	std::cout << "원\r\n";
	
	int r = 20;
	int powR = pow(r / 2, 2); // 맨 처음만 실행하면 되는 연산
	int powY; // for문 한번 돌 때마다 실행하면 되는 연산

	for (int i = 0; i < r; ++++i)
	{
		powY = pow(i - r / 2, 2);

		for (int j = 0; j < r; ++j)
		{
			std::cout << ((powY + pow(j - r / 2, 2) < powR) ? "*" : " ");

			// if(powY + pow(j - r / 2, 2) < powR)
			// {
			// 	std::cout << "*";
			// }
			// else
			// {
			// 	std::cout << " ";
			// }
		}

		std::cout << "\r\n";
	}

	return(0);
}
