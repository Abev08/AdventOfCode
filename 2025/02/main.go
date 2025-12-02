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

	invalidIDSumOne, invalidIDSumTwo := 0, 0

	for scanner.Scan() {
		t := scanner.Text()
		if len(t) == 0 {
			continue
		}

		for s := range strings.SplitSeq(t, ",") {
			if len(s) == 0 {
				continue
			}

			r := strings.Split(s, "-")
			rBottom, _ := strconv.ParseInt(r[0], 10, 64)
			rTop, _ := strconv.ParseInt(r[1], 10, 64)

			for id := rBottom; id <= rTop; id++ {
				sID := strconv.Itoa(int(id))
				sIDLen := len(sID)

				// Part one
				if sIDLen%2 == 0 {
					lID := sID[:sIDLen/2]
					rID := sID[sIDLen/2:]
					if lID == rID {
						invalidIDSumOne += int(id)
					}
				}

				// Part two
				for l := 1; l <= sIDLen/2; l++ {
					if sIDLen%l != 0 {
						continue
					}

					tmp := make([]string, sIDLen/l)
					idx := 0
					for start := 0; start <= sIDLen-l; start += l {
						tmp[idx] = sID[start : start+l]
						idx++
					}

					invalid := true
					for i := 1; i < len(tmp); i++ {
						if tmp[0] != tmp[i] {
							invalid = false
							break
						}
					}
					if invalid {
						invalidIDSumTwo += int(id)
						break
					}
				}
			}
		}
	}

	fmt.Println("Part one answer:", invalidIDSumOne)
	fmt.Println("Part two answer:", invalidIDSumTwo)
}
