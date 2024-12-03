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

	// Parse the input
	var sum, sum2 int64
	enabled := true
	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		l := scanner.Text()
		if len(l) == 0 {
			continue
		}

		// Part one
		line := l
		for {
			idx := strings.Index(line, "mul(")
			if idx == -1 {
				break
			}
			line = line[idx+4:]
			n1, n2, ok := checkMul(line)
			if ok {
				sum += n1 * n2
			}
		}

		// Part two
		line = l
		for {
			idxMul := strings.Index(line, "mul(")
			if idxMul == -1 {
				break
			}
			idxDo := strings.Index(line, "do()")
			idxDont := strings.Index(line, "don't()")
			minIdx := min(
				tif(idxMul >= 0, idxMul, math.MaxInt32),
				tif(idxDo >= 0, idxDo, math.MaxInt32),
				tif(idxDont >= 0, idxDont, math.MaxInt32))
			switch minIdx {
			case idxMul:
				line = line[idxMul+4:]
				if enabled {
					n1, n2, ok := checkMul(line)
					if ok {
						sum2 += n1 * n2
					}
				}
			case idxDo:
				line = line[idxDo+4:]
				enabled = true
			case idxDont:
				line = line[idxDont+7:]
				enabled = false
			}
		}
	}

	fmt.Println("Part one answer:", sum)
	fmt.Println("Part two answer:", sum2)
}

func checkMul(line string) (int64, int64, bool) {
	num1Start, num1End := 0, 0 // Indexes at which numbers start and end
	num2Start, num2End := 0, 0
	for i, c := range line {
		if c >= '0' && c <= '9' {
			if num1End != 0 && num2Start == 0 {
				num2Start = i
			}
			continue
		} else if c == ',' {
			if num1End != 0 {
				return 0, 0, false
			}
			num1End = i
		} else if c == ')' {
			num2End = i
			break
		} else {
			return 0, 0, false
		}
	}

	n1, _ := strconv.ParseInt(line[num1Start:num1End], 10, 32)
	n2, _ := strconv.ParseInt(line[num2Start:num2End], 10, 32)
	return n1, n2, true
}

func tif[T any](condition bool, valueTrue, valueFalse T) T {
	if condition {
		return valueTrue
	}
	return valueFalse
}
