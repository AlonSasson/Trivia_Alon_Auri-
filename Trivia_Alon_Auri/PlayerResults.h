#pragma once
#include <string>
typedef struct PlayerResults
{
	std::string username;
	unsigned int score;
	unsigned int correctAnswerCount;
	unsigned int wrongAnswerCount;
	double averageAnswerTime;

} PlayerResults;
