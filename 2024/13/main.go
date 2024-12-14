package main

import (
	"bufio"
	"fmt"
	"math"
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

	tokens1, tokens2 := 0, 0
	// Parse the input
	scanner := bufio.NewScanner(file)
	a, b := Vec2{}, Vec2{}
	for scanner.Scan() {
		line := scanner.Text()
		if len(line) == 0 {
			continue
		}
		idxSep := strings.Index(line, ",")
		idxX := strings.Index(line, "X")
		idxY := strings.Index(line, "Y")
		valX := parseFloat(line[idxX+2 : idxSep])
		valY := parseFloat(line[idxY+2:])
		if line[:1] == "B" { // Button line
			if line[7:8] == "A" { // Button A
				a.X, a.Y = valX, valY
			} else { // Button B
				b.X, b.Y = valX, valY
			}
		} else {
			bCount := (valY*a.X - valX*a.Y) / (b.Y*a.X - b.X*a.Y)
			aCount := (valY - b.Y*bCount) / a.Y
			if aCount <= 100 && bCount <= 100 &&
				math.Trunc(aCount) == aCount &&
				math.Trunc(bCount) == bCount {
				tokens1 += int(aCount)*3 + int(bCount)
			}

			valX += 10000000000000
			valY += 10000000000000
			bCount = (valY*a.X - valX*a.Y) / (b.Y*a.X - b.X*a.Y)
			aCount = (valY - b.Y*bCount) / a.Y
			if math.Trunc(aCount) == aCount &&
				math.Trunc(bCount) == bCount {
				tokens2 += int(aCount)*3 + int(bCount)
			}
		}
	}

	fmt.Println("Part one answer:", tokens1)
	fmt.Println("Part two answer:", tokens2)
}

type Vec2 struct {
	X, Y float64
}

func parseFloat(s string) float64 {
	num, err := strconv.ParseFloat(s, 64)
	if err != nil {
		panic(err)
	}
	return num
}
