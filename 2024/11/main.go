package main

import (
	"bufio"
	"fmt"
	"os"
	"strconv"
	"strings"
)

type CacheVal struct {
	Stone          int
	StepsRemaining int
}

var Cache = make(map[CacheVal]int)

func main() {
	// Open file
	file, err := os.Open("input.txt")
	if err != nil {
		panic(err)
	}
	defer file.Close()

	// Parse the input
	scanner := bufio.NewScanner(file)
	scanner.Scan()
	numbers := strings.Split(scanner.Text(), " ")
	stones := make([]int, len(numbers))
	for i := range numbers {
		n, _ := strconv.ParseInt(numbers[i], 10, 64)
		stones[i] = int(n)
	}

	// Count the stones after x blinks
	steps := []int{25, 75}
	for i := range steps {
		stonesCount := 0
		for _, stone := range stones {
			stonesCount += countStones(stone, steps[i])
		}

		if steps[i] == 25 {
			fmt.Println("Part one answer:", stonesCount)
		} else {
			fmt.Println("Part two answer:", stonesCount)
		}
	}
}

func countStones(stone, steps int) int {
	if steps == 0 {
		return 1
	}
	if stone == 0 { // 1st rule
		return countStones(1, steps-1)
	}
	val, ok := Cache[CacheVal{Stone: stone, StepsRemaining: steps}]
	if !ok {
		str := strconv.Itoa(stone)
		length := len(str)
		if length%2 == 0 { // 2nd rule
			n1, _ := strconv.ParseInt(str[:length/2], 10, 64)
			n2, _ := strconv.ParseInt(str[length/2:], 10, 64)
			val = countStones(int(n1), steps-1) + countStones(int(n2), steps-1)
		} else {
			val = countStones(stone*2024, steps-1) // 3rd rule
		}
		Cache[CacheVal{Stone: stone, StepsRemaining: steps}] = val // Cache the result
	}
	return val
}
