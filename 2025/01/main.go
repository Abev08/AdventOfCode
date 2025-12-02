package main

import (
	"bufio"
	"fmt"
	"os"
	"strconv"
)

func main() {
	file, _ := os.Open("input.txt")
	scanner := bufio.NewScanner(file)

	dial, stoppedAtZeroCounter, passedZeroCounter := 50, 0, 0
	for scanner.Scan() {
		t := scanner.Text()
		if len(t) == 0 {
			continue
		}

		dir := t[0]
		val, _ := strconv.ParseInt(t[1:], 10, 64)

		prevDial := dial
		for range val {
			switch dir {
			case 'L':
				dial--
			case 'R':
				dial++
			}

			if prevDial == 0 {
				passedZeroCounter++
			}
			dial = dial % 100
			for dial < 0 {
				dial += 100
			}
			prevDial = dial
		}

		if dial == 0 {
			stoppedAtZeroCounter++
		}
	}

	fmt.Println("Part one answer:", stoppedAtZeroCounter)
	fmt.Println("Part two answer:", passedZeroCounter)
}
