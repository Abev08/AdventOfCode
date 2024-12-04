package main

import (
	"bufio"
	"fmt"
	"os"
)

type CharAtOffset struct {
	Char             rune
	XOffset, YOffset int
}
var xmas = [][]CharAtOffset{
	{{'M', -1, 0}, {'A', -2, 0}, {'S', -3, 0}},    // Left
	{{'M', -1, -1}, {'A', -2, -2}, {'S', -3, -3}}, // Top left
	{{'M', 0, -1}, {'A', 0, -2}, {'S', 0, -3}},    // Top
	{{'M', 1, -1}, {'A', 2, -2}, {'S', 3, -3}},    // Top right
	{{'M', 1, 0}, {'A', 2, 0}, {'S', 3, 0}},       // Right
	{{'M', 1, 1}, {'A', 2, 2}, {'S', 3, 3}},       // Bottom right
	{{'M', 0, 1}, {'A', 0, 2}, {'S', 0, 3}},       // Bottom
	{{'M', -1, 1}, {'A', -2, 2}, {'S', -3, 3}},    // Bottom left
}

func main() {
	// Open file
	file, err := os.Open("input.txt")
	if err != nil {
		panic(err)
	}
	defer file.Close()

	// Parse the input
	var charMap [][]rune = make([][]rune, 0, 140)
	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		l := scanner.Text()
		if len(l) == 0 {
			continue
		}
		line := make([]rune, len(l))
		for i, c := range l {
			line[i] = c
		}
		charMap = append(charMap, line)
	}

	xmasCounter, masCounter := 0, 0
	for y := range charMap {
		line := charMap[y]
		for x := range line {
			// Part one
			if line[x] == 'X' {
				xmasCounter += lookForKeyword(charMap, x, y)
			}

			// Part two
			if line[x] == 'A' {
				if charExpected('M', charMap, x-1, y-1) && charExpected('S', charMap, x+1, y+1) {
					if charExpected('M', charMap, x+1, y-1) && charExpected('S', charMap, x-1, y+1) {
						masCounter++
					} else if charExpected('S', charMap, x+1, y-1) && charExpected('M', charMap, x-1, y+1) {
						masCounter++
					}
				} else if charExpected('S', charMap, x-1, y-1) && charExpected('M', charMap, x+1, y+1) {
					if charExpected('S', charMap, x+1, y-1) && charExpected('M', charMap, x-1, y+1) {
						masCounter++
					} else if charExpected('M', charMap, x+1, y-1) && charExpected('S', charMap, x-1, y+1) {
						masCounter++
					}
				}
			}
		}
	}

	fmt.Println("Part one answer:", xmasCounter)
	fmt.Println("Part two answer:", masCounter)
}

func lookForKeyword(charMap [][]rune, x, y int) int {
	foundCount := 0
	for i := range xmas {
		for j := range xmas[i] {
			exp := xmas[i][j]
			if charExpected(exp.Char, charMap, x+exp.XOffset, y+exp.YOffset) {
				if j == 2 {
					foundCount++
				}
			} else {
				break
			}
		}
	}
	return foundCount
}

func charExpected(char rune, charMap [][]rune, x, y int) bool {
	if x < 0 || y < 0 {
		return false
	}
	if x >= len(charMap[0]) || y >= len(charMap) {
		return false
	}
	if charMap[y][x] == char {
		return true
	}
	return false
}
