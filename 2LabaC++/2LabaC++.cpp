#define _CRT_SECURE_NO_WARNINGS
#include <iostream>
#include <unordered_map>

using namespace std;

string mostCommonWord(const string & path) {
	if (!freopen(path.c_str(), "r", stdin)) {
		std::cerr << "Не удалось открыть файл '" << path
			<< "': " << std::strerror(errno) << "\n";
		return "error";
	}

	string s;
	unordered_map<string, long long> cnt;
	
	while (cin >> s) {
		cnt[s]++;
	}

	long long best = 0;
	string bestWord;
	for (auto& p : cnt) {
		if (p.second > best) {
			best = p.second;
			bestWord = p.first;
		}
	}
	return bestWord;
}

int main()
{
	cout << mostCommonWord("..\\2Laba\\History for ready reference, Volume 1, A-Elba by J. N. Larned.txt");
}
