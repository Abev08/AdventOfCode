package main

import (
	"bufio"
	"fmt"
	"os"
	"strings"
)

func main() {
	file, _ := os.Open("input.txt")
	defer file.Close()
	scanner := bufio.NewScanner(file)

	partOne, partTwo := 0, int64(0)
	grid := make([][]byte, 0, 10)

	for scanner.Scan() {
		line := scanner.Text()
		if len(line) == 0 {
			continue
		}

		grid = append(grid, []byte(line))
	}

	// Copy the grid for part two, also convert it into int64 numbers
	partTwoGrid := make([][]int64, len(grid))
	for i, g := range grid {
		partTwoGrid[i] = make([]int64, len(g))
		if i == 0 {
			idx := strings.Index(string(g), "S")
			partTwoGrid[i][idx] = 1
			continue
		}

		for idx, gg := range g {
			if gg == '^' {
				partTwoGrid[i][idx] = -1 // Splitter marked as -1
			}
		}
	}

	// Part one
	currentRow := 0
	for currentRow < len(grid)-1 {
		if currentRow == 0 {
			startIdx := strings.Index(string(grid[currentRow]), "S")
			currentRow++
			grid[currentRow][startIdx] = '|'
		}

		for idx, r := range grid[currentRow] {
			if r == '|' {
				switch grid[currentRow+1][idx] {
				case '.':
					grid[currentRow+1][idx] = '|'
				case '^':
					splitted := false
					if idx > 0 && grid[currentRow+1][idx-1] == '.' {
						grid[currentRow+1][idx-1] = '|'
						splitted = true
					}
					if idx < len(grid[currentRow+1])-1 && grid[currentRow+1][idx+1] == '.' {
						grid[currentRow+1][idx+1] = '|'
						splitted = true
					}
					if splitted {
						partOne++
					}
				}
			}
		}

		currentRow++
	}

	// Part two
	currentRow = 0
	for currentRow < len(partTwoGrid)-1 {
		for idx, val := range partTwoGrid[currentRow] {
			if val <= 0 {
				continue
			}

			if partTwoGrid[currentRow+1][idx] != -1 {
				partTwoGrid[currentRow+1][idx] += val
			} else { // Split
				partTwoGrid[currentRow+1][idx-1] += val
				partTwoGrid[currentRow+1][idx+1] += val
			}
		}

		currentRow++
	}
	// Sum up the end values
	for _, val := range partTwoGrid[len(partTwoGrid)-1] {
		if val <= 0 {
			continue
		}
		partTwo += val
	}

	fmt.Println("Part one answer:", partOne)
	fmt.Println("Part two answer:", partTwo)
}
