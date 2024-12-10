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
	scanner := bufio.NewScanner(file)
	antennas := make(map[rune][]Antena)
	boundaryX, y := 0, 0
	for scanner.Scan() {
		line := scanner.Text()
		if len(line) == 0 {
			break
		}
		boundaryX = len(line)
		for x, c := range line {
			if c != '.' {
				ant := antennas[c]
				ant = append(ant, Antena{Symbol: c, Pos: Vec2{X: x, Y: y}})
				antennas[c] = ant
			}
		}
		y++
	}
	boundaryY := y

	// Part one
	antinodes := make([]Vec2, 0, 1000)
	for _, ant := range antennas {
		for idx1 := 0; idx1 < len(ant)-1; idx1++ {
			a1 := ant[idx1]
			for idx2 := idx1 + 1; idx2 < len(ant); idx2++ {
				a2 := ant[idx2]
				dif := a1.Pos.Minus(a2.Pos)
				antinodeA1 := Vec2{X: a1.Pos.X + dif.X, Y: a1.Pos.Y + dif.Y}
				antinodeA2 := Vec2{X: a2.Pos.X - dif.X, Y: a2.Pos.Y - dif.Y}
				if antinodeA1.X >= 0 && antinodeA1.X < boundaryX &&
					antinodeA1.Y >= 0 && antinodeA1.Y < boundaryY {
					antinodes = AddUniqueVec2(antinodes, antinodeA1)
				}
				if antinodeA2.X >= 0 && antinodeA2.X < boundaryX &&
					antinodeA2.Y >= 0 && antinodeA2.Y < boundaryY {
					antinodes = AddUniqueVec2(antinodes, antinodeA2)
				}
			}
		}
	}
	fmt.Println("Part one answer:", len(antinodes))

	// Part two
	antinodes = antinodes[:0]
	for _, ant := range antennas {
		for idx1 := 0; idx1 < len(ant)-1; idx1++ {
			a1 := ant[idx1]
			for idx2 := idx1 + 1; idx2 < len(ant); idx2++ {
				a2 := ant[idx2]
				dif := a1.Pos.Minus(a2.Pos)
				antinodeA1 := Vec2{X: a1.Pos.X + dif.X, Y: a1.Pos.Y + dif.Y}
				for {
					if antinodeA1.X >= 0 && antinodeA1.X < boundaryX &&
						antinodeA1.Y >= 0 && antinodeA1.Y < boundaryY {
						antinodes = AddUniqueVec2(antinodes, antinodeA1)
					} else {
						break
					}
					antinodeA1.X += dif.X
					antinodeA1.Y += dif.Y
				}
				antinodeA2 := Vec2{X: a2.Pos.X - dif.X, Y: a2.Pos.Y - dif.Y}
				for {
					if antinodeA2.X >= 0 && antinodeA2.X < boundaryX &&
						antinodeA2.Y >= 0 && antinodeA2.Y < boundaryY {
						antinodes = AddUniqueVec2(antinodes, antinodeA2)
					} else {
						break
					}
					antinodeA2.X -= dif.X
					antinodeA2.Y -= dif.Y
				}
				antinodes = AddUniqueVec2(antinodes, a1.Pos)
				antinodes = AddUniqueVec2(antinodes, a2.Pos)
			}
		}
	}
	fmt.Println("Part two answer:", len(antinodes))
}

type Vec2 struct {
	X, Y int
}

func (v *Vec2) Minus(vec Vec2) Vec2 {
	return Vec2{v.X - vec.X, v.Y - vec.Y}
}

func AddUniqueVec2(arr []Vec2, vec Vec2) []Vec2 {
	for i := range arr {
		a := arr[i]
		if vec.X == a.X && vec.Y == a.Y {
			return arr
		}
	}
	return append(arr, Vec2{vec.X, vec.Y})
}

type Antena struct {
	Symbol rune
	Pos    Vec2
}

func Tif[T any](condition bool, vTrue, vFalse T) T {
	if condition {
		return vTrue
	}
	return vFalse
}
