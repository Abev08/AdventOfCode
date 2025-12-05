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

	partOne, partTwo := 0, uint64(0)
	parsingRanges := true
	ranges := make([]Range, 0, 10)

	for scanner.Scan() {
		line := scanner.Text()
		if len(line) == 0 {
			parsingRanges = false
			continue
		}

		if parsingRanges {
			r := strings.Split(line, "-")
			rB, _ := strconv.ParseUint(r[0], 10, 64)
			rT, _ := strconv.ParseUint(r[1], 10, 64)
			ranges = append(ranges, Range{rB, rT})
		} else {
			id, _ := strconv.ParseUint(line, 10, 64)

			for _, r := range ranges {
				if id >= r.bottom && id <= r.top {
					partOne++
					break
				}
			}
		}
	}
	fmt.Println("Part one answer:", partOne)

	// Part two
	// Clean up the ranges
	newRanges := make([]Range, 0, len(ranges))
	for i, r := range ranges {
		tmp := make([]Range, 1, 10)
		tmp[0] = Range{r.bottom, r.top}

		for j := i - 1; j >= 0; j-- {
			rr := ranges[j]

			for k, kk := range tmp {
				if kk.bottom >= rr.bottom && kk.top <= rr.top {
					// Existing range contains this one, reset the values and don't add to existing ranges
					tmp[k] = Range{}
					break
				}
				if kk.bottom >= rr.bottom && kk.bottom <= rr.top {
					// Existing range contains lower part of this one, move the bottom value
					tmp[k].bottom = rr.top + 1
				}
				if kk.top >= rr.bottom && kk.top <= rr.top {
					// Existing range contains higher part of this one, move the top value
					tmp[k].top = rr.bottom - 1
				}
				if kk.bottom <= rr.bottom && kk.top >= rr.top {
					// This one contains existing range, split current range into 2 ranges
					tmp[k].bottom, tmp[k].top = kk.bottom, rr.bottom-1
					tmp = append(tmp, Range{rr.top + 1, kk.top})
				}
			}
		}

		for _, kk := range tmp {
			if kk.bottom == 0 && kk.top == 0 {
				continue
			}
			newRanges = append(newRanges, kk)
		}
	}
	// Sum differences in new ranges
	for _, r := range newRanges {
		partTwo += r.top + 1 - r.bottom
	}
	fmt.Println("Part two answer:", partTwo)
}

type Range struct {
	bottom, top uint64
}
