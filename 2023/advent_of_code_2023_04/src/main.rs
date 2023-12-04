use std::{
  fs::File,
  io::{BufRead, BufReader},
};

fn main() {
  // Read input
  let mut input: Vec<String> = Vec::new();
  let file = File::open("input.txt").unwrap();
  for line in BufReader::new(file).lines() {
    input.push(line.unwrap());
  }

  // Part one
  let mut sum = 0;
  let mut start: usize;
  let mut separator: usize;
  let mut winning_numbers: Vec<Vec<i32>> = Vec::new();
  let mut numbers: Vec<Vec<i32>> = Vec::new();
  let mut count: i32;
  let mut idx: usize = 0;
  for line in input {
    winning_numbers.push(Vec::new());
    numbers.push(Vec::new());
    count = -1;
    start = line.find(':').unwrap() + 1;
    separator = line.find('|').unwrap();

    for num in line[start..separator].split(' ').map(|x| {
      return x.trim().parse::<i32>();
    }) {
      if num.is_ok() {
        winning_numbers[idx].push(num.unwrap());
      }
    }

    for num in line[(separator + 1)..].split(' ').map(|x| {
      return x.trim().parse::<i32>();
    }) {
      if num.is_ok() {
        numbers[idx].push(num.unwrap());
      }
    }

    for num in &numbers[idx] {
      if winning_numbers[idx].contains(&num) {
        count += 1;
      }
    }

    if count != -1 {
      sum += i32::pow(2, count as u32);
    }

    idx += 1;
  }
  println!("Part one answer: {sum}");

  // Part two
  let mut copies_count: Vec<i32> = Vec::new();
  let mut sum2 = 0;
  for _i in 0..winning_numbers.len() {
    copies_count.push(0);
  }
  for idx in 0..winning_numbers.len() {
    check_card(idx, &mut copies_count, &winning_numbers, &numbers);
  }
  for i in 0..copies_count.len() {
    sum2 += copies_count[i];
  }
  println!("Part two answer: {}", sum2);
}

fn check_card(
  idx: usize,
  copies_count: &mut Vec<i32>,
  winning_numbers: &Vec<Vec<i32>>,
  numbers: &Vec<Vec<i32>>,
) {
  copies_count[idx] += 1;
  let mut next_idx = idx + 1;
  for num in &numbers[idx] {
    if winning_numbers[idx].contains(&num) {
      check_card(next_idx, copies_count, winning_numbers, numbers);
      next_idx += 1;
    }
  }
}
