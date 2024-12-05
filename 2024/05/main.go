package main

import (
	"bufio"
	"fmt"
	"os"
	"slices"
	"strconv"
	"strings"
)

var rulesBefore, rulesAfter = make(map[int32][]int32), make(map[int32][]int32)

func main() {
	// Open file
	file, err := os.Open("input.txt")
	if err != nil {
		panic(err)
	}
	defer file.Close()

	// Parse the input
	var sum, sum2 int32
	parsingRules := true
	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		line := scanner.Text()
		if len(line) == 0 {
			parsingRules = false
			continue
		}

		if parsingRules {
			tmp := strings.Split(line, "|")
			n1 := parseInt32(tmp[0])
			n2 := parseInt32(tmp[1])
			// After
			r, ok := rulesAfter[n1]
			if ok {
				rulesAfter[n1] = append(r, n2)
			} else {
				rulesAfter[n1] = []int32{n2}
			}
			// Before
			r, ok = rulesBefore[n2]
			if ok {
				rulesBefore[n2] = append(r, n1)
			} else {
				rulesBefore[n2] = []int32{n1}
			}
		} else {
			// Check the pages
			tmp := strings.Split(line, ",")
			numbers := make([]int32, len(tmp))
			for i := range tmp {
				numbers[i] = parseInt32(tmp[i])
			}

			ok := true
			for i := range numbers {
				currNum := numbers[i]
				if i <= len(numbers) {
					// Check after
					rule, present := rulesAfter[currNum]
					if present {
						for j := i + 1; j < len(numbers); j++ {
							if !slices.Contains(rule, numbers[j]) {
								ok = false
								break
							}
						}
						if !ok {
							break
						}
					}
				}
				if i > 0 {
					// Check before
					rule, present := rulesBefore[currNum]
					if present {
						for j := i - 1; j >= 0; j-- {
							if !slices.Contains(rule, numbers[j]) {
								ok = false
								break
							}
						}
						if !ok {
							break
						}
					}
				}
			}
			if ok {
				sum += numbers[len(numbers)/2]
			} else {
				slices.SortFunc(numbers, rulesSort)
				sum2 += numbers[len(numbers)/2]
			}
		}
	}

	fmt.Println("Part one answer:", sum)
	fmt.Println("Part two answer:", sum2)
}

func parseInt32(s string) int32 {
	n1, _ := strconv.ParseInt(s, 10, 32)
	return int32(n1)
}

func rulesSort(a, b int32) int {
	// Check if a is after b
	after, present := rulesAfter[a]
	if present {
		if slices.Contains(after, b) {
			return 1
		}
	}
	// Check if a is before b
	before, present := rulesBefore[a]
	if present {
		if slices.Contains(before, b) {
			return -1
		}
	}
	return 0
}
