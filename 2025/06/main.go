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
	defer file.Close()
	scanner := bufio.NewScanner(file)

	partOne, partTwo := uint64(0), uint64(0)
	numbers := make([][]uint64, 0, 10)
	lines := make([]string, 0, 10)

	for scanner.Scan() {
		line := scanner.Text()
		if len(line) == 0 {
			continue
		}

		// Part one
		s := strings.Fields(line)
		if s[0][0] >= '0' && s[0][0] <= '9' {
			nums := make([]uint64, len(s))
			for i, ss := range s {
				nums[i], _ = strconv.ParseUint(ss, 10, 64)
			}
			numbers = append(numbers, nums)
		} else {
			// Parsing operations
			for i, ss := range s {
				sum := uint64(0)
				switch ss {
				case "*":
					for j := 0; j < len(numbers); j++ {
						if sum == 0 {
							sum = 1
						}
						sum *= numbers[j][i]
					}
				case "+":
					for j := 0; j < len(numbers); j++ {
						sum += numbers[j][i]
					}
				default:
					panic(ss)
				}

				partOne += sum
			}
		}

		// Part two
		if strings.HasPrefix(line, "*") || strings.HasPrefix(line, "+") {
			idxStart := 0
			for {
				idxEnd := strings.IndexAny(line[idxStart+1:], "*+")
				if idxEnd < 0 {
					idxEnd = len(line)
				} else {
					idxEnd += idxStart
				}

				count := idxEnd - idxStart
				nums := make([]uint64, count)
				sb := strings.Builder{}
				for i := range count {
					sb.Reset()
					for _, l := range lines {
						sb.WriteString(l[idxStart+i : idxStart+i+1])
					}
					nums[i], _ = strconv.ParseUint(strings.ReplaceAll(sb.String(), " ", ""), 10, 64)
				}

				sum := uint64(0)
				switch strings.ReplaceAll(line[idxStart:idxEnd], " ", "") {
				case "*":
					for _, n := range nums {
						if sum == 0 {
							sum = 1
						}
						sum *= n
					}
				case "+":
					for _, n := range nums {
						sum += n
					}
				default:
					panic("")
				}
				partTwo += sum

				idxStart = idxEnd + 1
				if idxStart >= len(line) {
					break
				}
			}
		} else {
			lines = append(lines, line)
		}
	}

	fmt.Println("Part one answer:", partOne)
	fmt.Println("Part two answer:", partTwo)
}
