package main

import (
	"bufio"
	"fmt"
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
	reportOKCounter := 0
	reportOKCounter2 := 0 // Part two counter
	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		line := scanner.Text()
		if len(line) == 0 {
			continue
		}

		v := strings.Split(line, " ")
		values := make([]int32, len(v))
		for i, s := range v {
			a, _ := strconv.ParseInt(s, 10, 32)
			values[i] = int32(a)
		}

		if checkReport(values) {
			reportOKCounter++
			reportOKCounter2++
		} else {
			// Try to remove every single value and recheck
			tmp := make([]int32, len(values)-1)
			for i := range values {
				idx := 0
				// Create copy of values slice without index 'i'
				for j := range values {
					if i == j {
						continue
					}
					tmp[idx] = values[j]
					idx++
				}
				if checkReport(tmp) {
					reportOKCounter2++
					break
				}
			}
		}
	}

	fmt.Println("Part one answer:", reportOKCounter)
	fmt.Println("Part two answer:", reportOKCounter2)
}

func checkReport(values []int32) bool {
	var prevDiff int32 = 0
	for i := 1; i < len(values); i++ {
		n1, n2 := values[i-1], values[i]
		diff := n1 - n2
		if diff == 0 || diff < -3 || diff > 3 {
			return false // Difference between values is 0 or more than 3
		} else if prevDiff != 0 && ((prevDiff < 0 && diff > 0) || (prevDiff > 0 && diff < 0)) {
			return false // Difference has changed from decreasing to increasing or vice versa
		}
		prevDiff = diff
	}

	return true
}
