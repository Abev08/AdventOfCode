use std::{
    collections::HashMap,
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

    // Part one
    let mut map: HashMap<&str, i32> = HashMap::new();
    map.insert("A X", 4); // rock-rock 1 -> draw 3 -> 4
    map.insert("A Y", 8); // rock-paper 2 -> win 6 -> 8
    map.insert("A Z", 3); // rock-scissors 3 -> lost 0 -> 3
    map.insert("B X", 1); // paper-rock 1 -> lost 0 -> 1
    map.insert("B Y", 5); // paper-paper 2 -> draw 3 -> 5
    map.insert("B Z", 9); // paper-scissors 3 -> win 6 -> 9
    map.insert("C X", 7); // scissors-rock 1 -> win 6 -> 7
    map.insert("C Y", 2); // scissors-paper 2 -> lost 0 -> 2
    map.insert("C Z", 6); // scissors-scissors 3 -> draw 3 -> 6
    let mut total: i32 = 0;
    for line in &input {
        total += map.get(line.as_str()).unwrap();
    }
    println!("Part one answer: {}", total);

    // Part two
    total = 0;
    map.clear();
    map.insert("A X", 3); // rock-scissors 3 -> lost 0 -> 3
    map.insert("A Y", 4); // rock-rock 1 -> draw 3 -> 4
    map.insert("A Z", 8); // rock-paper 2 -> win 6 -> 8
    map.insert("B X", 1); // paper-rock 1 -> lost 0 -> 1
    map.insert("B Y", 5); // paper-paper 2 -> draw 3 -> 5
    map.insert("B Z", 9); // paper-scissors 3 -> win 6 -> 9
    map.insert("C X", 2); // scissors-paper 2 -> lost 0 -> 2
    map.insert("C Y", 6); // scissors-scissors 3 -> draw 3 -> 6
    map.insert("C Z", 7); // scissors-rock 1 -> win 6 -> 7
    for line in &input {
        total += map.get(line.as_str()).unwrap();
    }
    println!("Part two answer: {}", total);
}
