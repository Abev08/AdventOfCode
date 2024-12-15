package main

import (
	"bufio"
	"fmt"
	"os"
)

func main() {
	// Open file
	file, err := os.Open("input.txt")
	if err != nil {
		panic(err)
	}
	defer file.Close()

	// Parse the input
	robot, robot2 := Vec2{}, Vec2{}
	warehouse, warehouse2 := make([][]rune, 0, 1000), make([][]rune, 0, 1000)
	y := 0
	parsingMoves := false
	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		line := scanner.Text()
		if len(line) == 0 {
			parsingMoves = true
			continue
		}

		if !parsingMoves {
			row := make([]rune, len(line))
			row2 := make([]rune, 0, len(line)*2)
			for x, c := range line {
				row[x] = c
				if c == '@' {
					robot.X, robot.Y = x, y
				}

				// Part two warehouse
				switch c {
				case '#':
					row2 = append(row2, '#')
					row2 = append(row2, '#')
				case 'O':
					row2 = append(row2, '[')
					row2 = append(row2, ']')
				case '.':
					row2 = append(row2, '.')
					row2 = append(row2, '.')
				case '@':
					robot2.X, robot2.Y = len(row2), y
					row2 = append(row2, '@')
					row2 = append(row2, '.')
				}
			}
			warehouse = append(warehouse, row)
			warehouse2 = append(warehouse2, row2)
			y++
			continue
		}

		for _, moveDir := range line {
			dir := Vec2{}
			switch moveDir {
			case '^':
				dir.Y = -1
			case '<':
				dir.X = -1
			case '>':
				dir.X = 1
			case 'v':
				dir.Y = 1
			}

			// Part one
			ok := false
			warehouse, ok = move(warehouse, robot, dir)
			if ok {
				warehouse[robot.Y][robot.X] = '.'
				robot = robot.Add(dir)
				warehouse[robot.Y][robot.X] = '@'
			}

			// Part two
			ok = false
			warehouse2, ok = move2(warehouse2, []Vec2{robot2}, dir)
			if ok {
				warehouse2[robot2.Y][robot2.X] = '.'
				robot2 = robot2.Add(dir)
				warehouse2[robot2.Y][robot2.X] = '@'
			}
		}
	}

	// Part one sum
	sum := 0
	for y, row := range warehouse {
		for x, spot := range row {
			if spot == 'O' {
				sum += (y * 100) + x
			}
		}
	}
	// for _, row := range warehouse {
	// 	fmt.Println(string(row))
	// }
	fmt.Println("Part one answer:", sum)

	// Part two sum
	sum = 0
	for y, row := range warehouse2 {
		for x, spot := range row {
			if spot == '[' {
				sum += (y * 100) + x
			}
		}
	}
	// for _, row := range warehouse2 {
	// 	fmt.Println(string(row))
	// }
	fmt.Println("Part two answer:", sum)
}

type Vec2 struct {
	X, Y int
}

func (v *Vec2) Add(vec Vec2) Vec2 {
	return Vec2{v.X + vec.X, v.Y + vec.Y}
}

func move(warehouse [][]rune, pos, dir Vec2) ([][]rune, bool) {
	nextPos := pos.Add(dir)
	nextSpot := warehouse[nextPos.Y][nextPos.X]
	switch nextSpot {
	case '#':
		return warehouse, false
	case '.':
		return warehouse, true
	case 'O', '[', ']':
		ok := false
		warehouse, ok = move(warehouse, nextPos, dir)
		if !ok {
			return warehouse, false
		}
		// Move current box
		nextNextPos := nextPos.Add(dir)
		warehouse[nextNextPos.Y][nextNextPos.X] = nextSpot
		return warehouse, true
	}

	return warehouse, false
}

func move2(warehouse [][]rune, pos []Vec2, dir Vec2) ([][]rune, bool) {
	if dir.Y == 0 {
		// Going left or right, modified move from part one can be used
		return move(warehouse, pos[0], dir)
	} else {
		nextPos := make([]Vec2, len(pos))
		nextSpot := make([]rune, len(pos))
		for i, p := range pos {
			nextPos[i] = p.Add(dir)
			nextSpot[i] = warehouse[nextPos[i].Y][nextPos[i].X]
		}

		ok := false
		if len(nextSpot) == 1 {
			switch nextSpot[0] {
			case '.':
				return warehouse, true
			case '#':
				return warehouse, false
			case '[':
				warehouse, ok = move2(warehouse,
					[]Vec2{nextPos[0], {nextPos[0].X + 1, nextPos[0].Y}},
					dir)
				if !ok {
					return warehouse, false
				}
				// Move current box
				warehouse[nextPos[0].Y+dir.Y][nextPos[0].X+dir.X] = '['
				warehouse[nextPos[0].Y+dir.Y][nextPos[0].X+1+dir.X] = ']'
				warehouse[nextPos[0].Y][nextPos[0].X] = '.'
				warehouse[nextPos[0].Y][nextPos[0].X+1] = '.'
				return warehouse, true
			case ']':
				warehouse, ok = move2(warehouse,
					[]Vec2{{nextPos[0].X - 1, nextPos[0].Y}, nextPos[0]},
					dir)
				if !ok {
					return warehouse, false
				}
				// Move current box
				warehouse[nextPos[0].Y+dir.Y][nextPos[0].X-1+dir.X] = '['
				warehouse[nextPos[0].Y+dir.Y][nextPos[0].X+dir.X] = ']'
				warehouse[nextPos[0].Y][nextPos[0].X-1] = '.'
				warehouse[nextPos[0].Y][nextPos[0].X] = '.'
				return warehouse, true
			}
		} else {
			// Check multiple positions
			allEmpty := true
			for _, n := range nextSpot {
				if n == '#' {
					return warehouse, false
				}
				if n != '.' {
					allEmpty = false
				}
			}
			if allEmpty {
				return warehouse, true
			}

			// Create next next pos to check
			nextNextPos := make([]Vec2, 0, 100)
			for i, spot := range nextSpot {
				switch spot {
				case '.': // Nothing
				case '[':
					nextNextPos = tryAdd(nextNextPos, nextPos[i])
					v := Vec2{nextPos[i].X + 1, nextPos[i].Y}
					nextNextPos = tryAdd(nextNextPos, v)
				case ']':
					v := Vec2{nextPos[i].X - 1, nextPos[i].Y}
					nextNextPos = tryAdd(nextNextPos, v)
					nextNextPos = tryAdd(nextNextPos, nextPos[i])
				}
			}
			warehouse, ok = move2(warehouse, nextNextPos, dir)
			if !ok {
				return warehouse, false
			}

			// Move the boxes
			for _, pos := range nextNextPos {
				newPos := pos.Add(dir)
				warehouse[newPos.Y][newPos.X] = warehouse[pos.Y][pos.X]
				warehouse[pos.Y][pos.X] = '.'
			}
			return warehouse, true
		}
	}
	return warehouse, false
}

func tryAdd(arr []Vec2, point Vec2) []Vec2 {
	for _, v := range arr {
		if v.X == point.X && v.Y == point.Y {
			return arr
		}
	}
	arr = append(arr, point)
	return arr
}
