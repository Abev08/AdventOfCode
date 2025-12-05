package main

import (
	"bufio"
	"fmt"
	"os"
	"strconv"
	"strings"
)

func main() {
	file, _ := os.Open("input.txt")
	scanner := bufio.NewScanner(file)

	const digits string = "9876543210"
	joltagePartOne, joltagePartTwo := uint64(0), uint64(0)

	for scanner.Scan() {
		line := scanner.Text()
		if len(line) == 0 {
			continue
		}

		// Part one
		maxValue := uint64(0)
		for _, d1 := range digits {
			firstIdx := strings.Index(line, string(d1))
			if firstIdx >= 0 {
				for _, d2 := range digits {
					secondIdx := strings.Index(line[firstIdx+1:], string(d2))
					if secondIdx >= 0 {
						val, _ := strconv.ParseUint(string(d1)+string(d2), 10, 64)
						if val > maxValue {
							maxValue = val
						}
					}
				}
			}
		}
		joltagePartOne += maxValue

		// Part two
		type battery struct {
			value byte
			index int
		}
		digits := [12]battery{}
		for i := range digits {
			digits[i] = battery{value: '9'}
		}
		for i := range digits {
			startIdx := 0
			if i > 0 {
				startIdx = digits[i-1].index + 1
			}
			for {
				idx := strings.Index(line[startIdx:], string(digits[i].value))
				if idx < 0 || idx+12-i > len(line[startIdx:]) {
					digits[i] = battery{digits[i].value - 1, 0}
					for j := i + 1; j < 12; j++ {
						digits[j].value = '9'
					}
				} else {
					digits[i] = battery{digits[i].value, idx + startIdx}
					break
				}
			}
		}

		sb := strings.Builder{}
		for i := range digits {
			sb.WriteByte(digits[i].value)
		}
		val, _ := strconv.ParseUint(sb.String(), 10, 64)
		joltagePartTwo += val
	}

	fmt.Println("Part one answer:", joltagePartOne)
	fmt.Println("Part two answer:", joltagePartTwo)
}
