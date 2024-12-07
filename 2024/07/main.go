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
	var sumResults1, sumResults2 int64
	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		line := scanner.Text()
		sepIdx := strings.Index(line, ":")

		result := parseInt64(line[:sepIdx])
		tmp := strings.Split(line[sepIdx+1:], " ")
		numbers := make([]int64, 0, len(tmp))
		for i := range tmp {
			if len(tmp[i]) == 0 {
				continue
			}
			numbers = append(numbers, parseInt64(tmp[i]))
		}

		// Part one
		part1OK := false
		numOperations := int64(math.Pow(2, float64(len(numbers)-1)))
		for op := int64(0); op < numOperations; op++ {
			var tmp int64
			for i := 0; i < len(numbers)-1; i++ {
				if (op & (1 << i)) > 0 {
					if i == 0 {
						tmp += numbers[i] * numbers[i+1]
					} else {
						tmp = tmp * numbers[i+1]
					}
				} else {
					if i == 0 {
						tmp += numbers[i] + numbers[i+1]
					} else {
						tmp = tmp + numbers[i+1]
					}
				}
			}
			if tmp == result {
				sumResults1 += result
				part1OK = true
				break
			}
		}

		// Part two
		if part1OK {
			sumResults2 += result
		} else {
			var op int64
			for op < (1 << (2 * (len(numbers) - 1))) {
				var tmp int64
				for i := 0; i < len(numbers)-1; i++ {
					oper := (op >> (2 * int64(i))) & 3
					if oper >= 3 {
						tmp = 0
						break
					}
					switch oper {
					case 0: // +
						if i == 0 {
							tmp += numbers[i] + numbers[i+1]
						} else {
							tmp = tmp + numbers[i+1]
						}
					case 1: // *
						if i == 0 {
							tmp += numbers[i] * numbers[i+1]
						} else {
							tmp = tmp * numbers[i+1]
						}
					case 2: // ||
						if i == 0 {
							tmp = parseInt64(fmt.Sprintf("%d%d", numbers[i], numbers[i+1]))
						} else {
							tmp = parseInt64(fmt.Sprintf("%d%d", tmp, numbers[i+1]))
						}
					default:
						panic("")
					}
				}
				if tmp == result {
					sumResults2 += result
					break
				}
				op++
			}
		}
	}

	fmt.Println("Part one answer:", sumResults1)
	fmt.Println("Part two answer:", sumResults2)
}

func parseInt64(s string) int64 {
	n, err := strconv.ParseInt(s, 10, 64)
	if err != nil {
		panic("Couldn't parse string into int64, s: " + s)
	}
	return n
}
