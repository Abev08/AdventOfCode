use std::{
    fs::File,
    io::{BufRead, BufReader},
};

fn main() {
    // Read the input file
    let mut input: Vec<String> = Vec::new();
    let f = File::open("input.txt");
    if f.is_ok() {
        let f = BufReader::new(f.unwrap());

        for line in f.lines() {
            let l = line.unwrap();
            // println!("{}", l);
            input.push(l);
        }
    } else {
        println!("{}", f.unwrap_err());
        return;
    }

    let mut parts: Vec<&str>;
    let mut values = [0, 0, 0, 0];
    let mut overlap_counter = 0;
    let mut overlap_counter_2 = 0;
    for line in &input {
        parts = line.split([',', '-']).collect();
        for i in 0..4 {
            values[i] = parts[i].parse().unwrap();
        }

        // Part one
        if values[0] >= values[2] && values[1] <= values[3] {
            overlap_counter += 1;
        } else if values[2] >= values[0] && values[3] <= values[1] {
            overlap_counter += 1;
        }

        // Part two
        if values[0] >= values[3] && values[1] <= values[2] {
            overlap_counter_2 += 1;
        } else if values[3] >= values[0] && values[2] <= values[1] {
            overlap_counter_2 += 1;
        }
    }
    println!("Part one answer: {}", overlap_counter);
    println!("Part two answer: {}", overlap_counter_2);
}
