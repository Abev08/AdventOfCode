package main

import (
	"bufio"
	"fmt"
	"os"
)

type Guard struct {
	PosX, PosY int  // Position
	VelX, VelY int  // Velocity
	dir        rune // Direction
}

func (g *Guard) Reset(x, y int) {
	g.PosX, g.PosY = x, y
	g.VelX, g.VelY = 0, -1
	g.dir = 'U'
}

func (g *Guard) Move() {
	for {
		nextPosX, nextPosY := g.PosX+g.VelX, g.PosY+g.VelY
		// Turn if facing obstacle
		if nextPosX >= 0 && nextPosX < len(floor[0]) &&
			nextPosY >= 0 && nextPosY < len(floor) &&
			floor[nextPosY][nextPosX] == '#' {
			switch g.dir {
			case 'U':
				g.VelX, g.VelY, g.dir = 1, 0, 'R'
			case 'R':
				g.VelX, g.VelY, g.dir = 0, 1, 'D'
			case 'D':
				g.VelX, g.VelY, g.dir = -1, 0, 'L'
			case 'L':
				g.VelX, g.VelY, g.dir = 0, -1, 'U'
			}
		} else {
			break
		}
	}
	g.PosX += g.VelX
	g.PosY += g.VelY
}

var floor = make([][]rune, 0, 100)

func main() {
	// Open file
	file, err := os.Open("input.txt")
	if err != nil {
		panic(err)
	}
	defer file.Close()

	// Parse the input
	var startPosX, startPosY int
	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		line := scanner.Text()
		row := make([]rune, len(line))
		for i, c := range line {
			row[i] = c
			if c == '^' {
				startPosX, startPosY = i, len(floor)
			}
		}
		floor = append(floor, row)
	}

	// Moving the guard
	guard := Guard{PosX: startPosX, PosY: startPosY, VelY: -1, dir: 'U'} // Going up
	for {
		if guard.PosX < 0 || guard.PosX >= len(floor[0]) ||
			guard.PosY < 0 || guard.PosY >= len(floor) {
			break
		}
		floor[guard.PosY][guard.PosX] = 'X'
		guard.Move()
	}

	checkedPlaces := 0
	for i := range floor {
		for _, c := range floor[i] {
			if c == 'X' {
				checkedPlaces++
			}
		}
	}
	fmt.Println("Part one answer:", checkedPlaces)

	// Part two
	infiniteLoops := 0
	for obstacleY := 0; obstacleY < len(floor); obstacleY++ {
		for obstacleX := 0; obstacleX < len(floor); obstacleX++ {
			if obstacleX == startPosX && obstacleY == startPosY {
				continue // Skip start position
			}
			guard.Reset(startPosX, startPosY)
			steps := 0
			prevChar := floor[obstacleY][obstacleX]
			floor[obstacleY][obstacleX] = '#'
			for {
				if steps > 10000 { // 10k steps was enough for my input :)
					infiniteLoops++
					break
				}

				if guard.PosX < 0 || guard.PosX >= len(floor[0]) ||
					guard.PosY < 0 || guard.PosY >= len(floor) {
					break
				}
				floor[guard.PosY][guard.PosX] = 'X'
				guard.Move()
				steps++
			}
			floor[obstacleY][obstacleX] = prevChar
		}
	}
	fmt.Println("Part two answer:", infiniteLoops)
}
