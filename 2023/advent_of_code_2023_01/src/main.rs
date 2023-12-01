use std::{
  fs,
  io::{BufRead, BufReader},
};

const NUMBERS: &[&str] = &[
  "one", "two", "three", "four", "five", "six", "seven", "eight", "nine",
];

fn main() {
  // Read input
  let mut input: Vec<String> = Vec::new();
  let file = fs::File::open("input.txt");
  let reader = BufReader::new(file.unwrap());
  for line in reader.lines() {
    input.push(line.unwrap());
  }

  // Part one
  let mut value: i32 = 0;
  let mut last_digit: i32 = 0;
  let mut first_digit: bool;
  for line in &input {
    first_digit = true;
    for char in line.chars() {
      if char.is_digit(10) {
        if first_digit {
          first_digit = false;
          value += char.to_digit(10).unwrap() as i32 * 10;
        }
        last_digit = char.to_digit(10).unwrap() as i32;
      }
    }
    value += last_digit;
  }
  println!("Part one answer: {}", value);

  // Part two
  value = 0;
  let mut index_end: usize;
  for line in &input {
    first_digit = true;
    last_digit = -1;
    index_end = 1;
    for char in line.chars() {
      if char.is_numeric() {
        // Just a number
        last_digit = char.to_digit(10).unwrap() as i32;
      } else {
        // Check if it's a word describing the number
        for index in (0..index_end).rev() {
          for i in 0..NUMBERS.len() {
            if NUMBERS[i] == &line[index..index_end] {
              last_digit = i as i32 + 1;
              break;
            }
          }
        }
      }
      index_end += 1;

      if first_digit && last_digit > 0 {
        first_digit = false;
        value += last_digit * 10;
      }
    }
    value += last_digit;
  }
  println!("Part two answer: {}", value);
}
