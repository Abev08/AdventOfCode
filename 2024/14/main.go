package main

import (
	"bufio"
	"fmt"
	"os"
	"strconv"
	"strings"
)

func main() {
	// Open file
	file, err := os.Open("input.txt")
	if err != nil {
		panic(err)
	}
	defer file.Close()

	// Parse the input
	robots := make([]Robot, 0, 1000)
	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		line := scanner.Text()
		if len(line) == 0 {
			continue
		}

		r := Robot{}
		txt := strings.Split(line, " ")
		idx1, idx2 := strings.Index(txt[0], "="), strings.Index(txt[0], ",")
		r.Position.X, r.Position.Y = parseInt(txt[0][idx1+1:idx2]), parseInt(txt[0][idx2+1:])
		idx1, idx2 = strings.Index(txt[1], "="), strings.Index(txt[1], ",")
		r.Velocity.X, r.Velocity.Y = parseInt(txt[1][idx1+1:idx2]), parseInt(txt[1][idx2+1:])
		robots = append(robots, r)
	}

	width, height := 101, 103 // Size of the room
	for second := range 10000 {
		for i := range robots {
			robots[i].Move(width, height)
		}

		if second == 99 {
			// Part one
			quadrants := make([]int, 4)
			halfWidth, halfHeight := width/2, height/2
			for _, r := range robots {
				if r.Position.X < halfWidth && r.Position.Y < halfHeight {
					quadrants[0]++
				} else if r.Position.X > halfWidth && r.Position.Y < halfHeight {
					quadrants[1]++
				} else if r.Position.X > halfWidth && r.Position.Y > halfHeight {
					quadrants[2]++
				} else if r.Position.X < halfWidth && r.Position.Y > halfHeight {
					quadrants[3]++
				}
			}
			safetyFactor := quadrants[0] * quadrants[1] * quadrants[2] * quadrants[3]
			fmt.Println("Part one answer:", safetyFactor)
		}

		// Part two, lets look for robots grouped together
		for _, r := range robots {
			pos := r.Position
			ok := 0
			for _, r2 := range robots {
				pos2 := r2.Position
				if abs(pos2.X-pos.X) < 3 && abs(pos2.Y-pos.Y) < 3 { // lets try 5x5 square
					ok++
				}
			}
			if ok >= 20 { // At least 20 robots are close to each other, maybe an easter egg?
				printArea(robots, width, height)
				fmt.Println("Part two answer:", second+1)
				return
			}
		}
	}
}

type Vec2 struct {
	X, Y int
}

type Robot struct {
	Position Vec2
	Velocity Vec2
}

func (r *Robot) Move(width, height int) {
	r.Position.X += r.Velocity.X
	r.Position.Y += r.Velocity.Y
	for r.Position.X < 0 {
		r.Position.X += width
	}
	for r.Position.Y < 0 {
		r.Position.Y += height
	}
	for r.Position.X >= width {
		r.Position.X -= width
	}
	for r.Position.Y >= height {
		r.Position.Y -= height
	}
}

func parseInt(s string) int {
	num, err := strconv.ParseInt(s, 10, 32)
	if err != nil {
		panic(err)
	}
	return int(num)
}

func printArea(robots []Robot, width, height int) {
	area := make([][]int, height)
	for i := range area {
		area[i] = make([]int, width)
	}
	for _, r := range robots {
		area[r.Position.Y][r.Position.X]++
	}
	for _, ar := range area {
		for _, a := range ar {
			if a == 0 {
				fmt.Print(" ")
			} else {
				fmt.Print(a)
			}
		}
		fmt.Println()
	}
	fmt.Println()
}

func abs(val int) int {
	if val < 0 {
		return -val
	}
	return val
}
