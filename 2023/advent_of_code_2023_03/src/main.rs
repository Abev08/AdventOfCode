use std::{
  fs::File,
  io::{BufRead, BufReader},
};

#[derive(Debug, PartialEq)]
struct Number {
  value: i32,
  row: usize,
  start_column: usize,
  end_column: usize,
}

fn main() {
  // Read input
  let mut input: Vec<String> = Vec::new();
  let file = File::open("input.txt").unwrap();
  for line in BufReader::new(file).lines() {
    input.push(line.unwrap());
  }

  // Parse the numbers
  let rows = input.len();
  let columns = input[0].len();
  let mut symbol: char;
  let mut start: usize = usize::MAX;
  let mut numbers: Vec<Vec<Number>> = Vec::new();
  for row in 0..rows {
    numbers.push(Vec::new());
    for column in 0..columns {
      symbol = input[row].chars().nth(column).unwrap();
      if symbol.is_digit(10) {
        if start == usize::MAX {
          start = column;
        }
      } else if start != usize::MAX {
        numbers[row].push(Number {
          value: input[row][start..column].parse::<i32>().unwrap(),
          row: row,
          start_column: start,
          end_column: column,
        });
        start = usize::MAX;
      }
    }
    if start != usize::MAX {
      numbers[row].push(Number {
        value: input[row][start..columns].parse::<i32>().unwrap(),
        row: row,
        start_column: start,
        end_column: columns,
      });
      start = usize::MAX;
    }
  }

  // Part one / two
  let mut parts: Vec<&Number> = Vec::new();
  let mut cogs: Vec<&Number> = Vec::new();
  let mut sum = 0;
  let mut sum2 = 0;
  for row in 0..rows {
    for column in 0..columns {
      symbol = input[row].chars().nth(column).unwrap();
      if symbol != '.' && !symbol.is_digit(10) {
        // Found a symbol, look for adjacent numbers
        cogs.clear();
        for rr in (row.saturating_add_signed(-1))..(row + 2) {
          if rr > rows {
            continue;
          }
          for i in 0..numbers[rr].len() {
            if (column >= numbers[rr][i].start_column.saturating_add_signed(-1))
              && column < numbers[rr][i].end_column + 1
            {
              // Part one
              if !parts.contains(&&numbers[rr][i]) {
                parts.push(&numbers[rr][i]);
              }

              // Part two
              if symbol == '*' {
                if !cogs.contains(&&numbers[rr][i]) {
                  cogs.push(&numbers[rr][i]);
                }
              }
            }
          }
        }
        // Part two - if 2 cogs are adjacent to '*' add their gear ratio
        if cogs.len() == 2 {
          sum2 += cogs[0].value * cogs[1].value;
        }
      }
    }
  }
  // Part one - sum the part numbers
  for i in 0..parts.len() {
    sum += parts[i].value;
  }

  println!("Part one answer: {sum}");
  println!("Part two answer: {sum2}");
}
