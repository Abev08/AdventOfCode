package main

import (
	"bufio"
	"fmt"
	"os"
)

func main() {
	file, _ := os.Open("input.txt")
	scanner := bufio.NewScanner(file)

	partOne, partTwo := 0, 0
	grid := make([][]rune, 0, 10)

	for scanner.Scan() {
		line := scanner.Text()
		if len(line) == 0 {
			continue
		}

		g := make([]rune, len(line))
		for i, c := range line {
			g[i] = c
		}
		grid = append(grid, g)
	}

	w, h := len(grid[0]), len(grid)
	topLoopCounter, previousPartTwo := 0, 0

	for {
		gridCopy := make([][]rune, h)
		for i, g := range grid {
			gridCopy[i] = make([]rune, w)
			copy(gridCopy[i], g)
		}

		for y := range h {
			for x := range w {
				if grid[y][x] != '@' {
					continue
				}

				neighbor := 0
				if y > 0 && x > 0 && grid[y-1][x-1] == '@' { //  top left
					neighbor++
				}
				if y > 0 && grid[y-1][x] == '@' { // top
					neighbor++
				}
				if y > 0 && x < w-1 && grid[y-1][x+1] == '@' { //  top right
					neighbor++
				}
				if x > 0 && grid[y][x-1] == '@' { // left
					neighbor++
				}
				if x < w-1 && grid[y][x+1] == '@' { // right
					neighbor++
				}
				if y < h-1 && x > 0 && grid[y+1][x-1] == '@' { //  bottom left
					neighbor++
				}
				if y < h-1 && grid[y+1][x] == '@' { // bottom
					neighbor++
				}
				if y < h-1 && x < w-1 && grid[y+1][x+1] == '@' { //  bottom right
					neighbor++
				}

				if neighbor < 4 {
					if topLoopCounter == 0 {
						partOne++
					}

					partTwo++
					gridCopy[y][x] = '.'
				}
			}
		}

		grid = gridCopy
		topLoopCounter++

		if previousPartTwo == partTwo {
			break
		}
		previousPartTwo = partTwo
	}

	fmt.Println("Part one answer:", partOne)
	fmt.Println("Part two answer:", partTwo)
}
