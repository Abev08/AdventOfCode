package main

import (
	"bufio"
	"fmt"
	"os"
	"slices"
	"strconv"
)

func main() {
	leftList := make([]int64, 0, 1000)
	rightList := make([]int64, 0, 1000)
	rightList2 := make(map[int64]int64)

	// Open file
	file, err := os.Open("input.txt")
	if err != nil {
		panic(err)
	}
	defer file.Close()

	// Parse the input
	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		line := scanner.Text()
		if len(line) == 0 {
			continue
		}

		var num1, num2 int64
		num1Readed := false
		for i, c := range line {
			if !num1Readed {
				if c == 32 {
					num1, _ = strconv.ParseInt(line[:i], 10, 64)
					num1Readed = true
				}
			} else {
				if c != 32 {
					num2, _ = strconv.ParseInt(line[i:], 10, 64)
					break
				}
			}
		}

		leftList = append(leftList, num1)
		rightList = append(rightList, num2)

		rightList2[num2] += 1 // Part two num counter
	}

	slices.Sort(leftList)
	slices.Sort(rightList)

	// Calculate answers
	var distance, similarity int64
	for i := 0; i < len(leftList); i++ {
		a, b := leftList[i], rightList[i]
		if a >= b {
			distance += a - b
		} else {
			distance += b - a
		}

		c, ok := rightList2[a]
		if ok {
			similarity += a * c
		}
	}

	fmt.Println("Part one answer:", distance)
	fmt.Println("Part two answer:", similarity)
}
