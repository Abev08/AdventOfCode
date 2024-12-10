package main

import (
	"bufio"
	"fmt"
	"math"
	"os"
	"strconv"
)

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
	line := scanner.Text()

	// Part one
	checksum := 0
	c2 := Cursor{Pos: len(line) - 1}
	c2.FileIndex = c2.Pos / 2
	c2.SizeLeft = parseInt(line[c2.Pos])
	c1 := Cursor{SizeLeft: parseInt(line[0])}
	moveCursor1 := true
	for {
		if moveCursor1 {
			if c1.SizeLeft > 0 {
				checksum += c1.FileIndex * c1.DiskPos
				c1.DiskPos++
				c1.SizeLeft--
			} else {
				c1.Pos++
				if c1.Pos > c2.Pos || c2.Pos < c1.Pos {
					break
				}
				c1.FreeSpace = !c1.FreeSpace
				c1.SizeLeft = parseInt(line[c1.Pos])
				if c2.Pos == c1.Pos+1 {
					c1.SizeLeft = math.MaxInt64
				}
				if c1.FreeSpace {
					if c1.SizeLeft > 0 {
						moveCursor1 = false
					}
				} else {
					c1.FileIndex++
				}
			}
		} else {
			// Move file from cursor 2 to cursor 1
			if c2.SizeLeft > 0 {
				checksum += c2.FileIndex * c1.DiskPos
				c1.DiskPos++
				c1.SizeLeft--
				c2.SizeLeft--
				if c1.SizeLeft <= 0 {
					// Out of free space, back to cursor 1
					moveCursor1 = true
				}
			} else {
				// Advance cursor 2 by 2 slots skipping free space
				c2.Pos -= 2
				if c1.Pos > c2.Pos || c2.Pos < c1.Pos {
					break
				}
				c2.FileIndex--
				c2.SizeLeft = parseInt(line[c2.Pos])
			}
		}
	}
	fmt.Println("Part one answer:", checksum)

	// Part two
	// Create disk layout
	disk := make([]Block, len(line))
	c1 = Cursor{}
	for _, char := range line {
		size := parseInt(byte(char))
		if c1.FreeSpace {
			disk[c1.Pos] = Block{FreeSpaceLeft: size}
		} else {
			files := make([]int, 0, size)
			for range size {
				files = append(files, c1.FileIndex)
			}
			c1.FileIndex++
			disk[c1.Pos] = Block{Files: files}
		}
		c1.Pos++
		c1.FreeSpace = !c1.FreeSpace
	}
	// Defragment the disk
	for idx := len(disk) - 1; idx > 0; idx-- {
		b1 := &disk[idx]
		requiredSize := len(b1.Files)
		if requiredSize == 0 {
			continue // Don't move free space
		}
		// Find new spot
		for i := 0; i < idx; i++ {
			b2 := &disk[i]
			if b2.FreeSpaceLeft >= requiredSize {
				// Move the file
				b2.Files = append(b2.Files, b1.Files...)
				b2.FreeSpaceLeft -= requiredSize
				b1.FreeSpaceLeft += requiredSize
				b1.Files = b1.Files[:0]
				break
			}
		}
	}
	// Calculate checksum
	checksum = 0
	diskPos := 0
	for i := 0; i < len(disk); i++ {
		b := &disk[i]
		for _, file := range b.Files {
			checksum += file * diskPos
			diskPos++
		}
		diskPos += b.FreeSpaceLeft
	}
	fmt.Println("Part two answer:", checksum)
}

func parseInt(b byte) int {
	i, err := strconv.ParseInt(string(b), 10, 32)
	if err != nil {
		panic(err)
	}
	return int(i)
}

type Cursor struct {
	Pos       int
	DiskPos   int
	FileIndex int
	SizeLeft  int
	FreeSpace bool
}

type Block struct {
	FreeSpaceLeft int
	Files         []int // File indexes like [1199]
}
