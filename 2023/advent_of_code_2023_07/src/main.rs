use std::{
  fs::File,
  io::{BufRead, BufReader},
};

const CARD_ORDER: &[char] = &[
  'A', 'K', 'Q', 'J', 'T', '9', '8', '7', '6', '5', '4', '3', '2',
];
const CARD_ORDER_2: &[char] = &[
  'A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2', 'J',
];

#[derive(Debug)]
struct Hand {
  cards: Vec<char>,
  hand_type: i32, // Five of a kind = 6, Four of a kind = 5, etc.
  bid: u32,
}

impl Hand {
  pub fn copy(&self) -> Hand {
    let mut h = Hand {
      cards: Vec::new(),
      hand_type: 0,
      bid: self.bid,
    };

    for i in 0..self.cards.len() {
      h.cards.push(self.cards[i]);
    }

    return h;
  }
}

fn main() {
  // Read input
  let mut hands: Vec<Hand> = Vec::new();
  let mut hands2: Vec<Hand> = Vec::new();
  let mut line: String = String::from("");
  let mut separator: usize;
  let file = File::open("input.txt").unwrap();
  for l in BufReader::new(file).lines() {
    line.clear();
    line.push_str(&l.unwrap());

    // Parsing the hand
    separator = line.find(' ').unwrap();
    let mut hand = Hand {
      cards: line[..separator].trim().chars().collect(),
      hand_type: 0,
      bid: line[separator..].trim().parse().unwrap(),
    };
    let mut hand2 = hand.copy();

    // Part one
    find_hand_type(&mut hand);
    add_with_sort(hand, &mut hands, CARD_ORDER);

    // Part two
    find_hand_type2(&mut hand2);
    add_with_sort(hand2, &mut hands2, CARD_ORDER_2);
  }

  let mut sum: u32 = 0;
  let mut sum2: u32 = 0;
  for i in 0..hands.len() {
    // Part one
    sum += hands[i].bid * (i as u32 + 1);

    // Part two
    sum2 += hands2[i].bid * (i as u32 + 1);
  }
  println!("Part one answer: {sum}");
  println!("Part two answer: {sum2}");
}

fn find_hand_type(hand: &mut Hand) {
  let mut cards: Vec<(char, i32)> = Vec::new();
  let mut new: bool;

  for idx in 0..hand.cards.len() {
    new = true;

    for i in 0..cards.len() {
      if hand.cards[idx] == cards[i].0 {
        new = false;
        cards[i].1 += 1;
        break;
      }
    }

    if new {
      cards.push((hand.cards[idx], 1));
    }
  }

  let mut hand_type = 0;

  for card in &cards {
    if card.1 == 2 {
      if hand_type == 0 {
        // first pair
        hand_type = 1;
      } else if hand_type == 1 {
        // second pair - two pairs
        hand_type = 2;
      } else if hand_type == 3 {
        // pair with 3 of kind - full house
        hand_type = 4;
      } else {
        panic!();
      }
    } else if card.1 == 3 {
      if hand_type == 0 {
        // first 3 of kind
        hand_type = 3
      } else if hand_type == 1 {
        // 3 of kind with a pair - full house
        hand_type = 4;
      } else {
        panic!();
      }
    } else if card.1 == 4 {
      if hand_type == 0 {
        // first 4 of kind
        hand_type = 5;
      } else {
        panic!();
      }
    } else if card.1 == 5 {
      if hand_type == 0 {
        // first 5 of kind
        hand_type = 6;
      } else {
        panic!();
      }
    }
  }

  hand.hand_type = hand_type;
}

fn find_hand_type2(hand: &mut Hand) {
  let mut cards: Vec<(char, i32)> = Vec::new();
  let mut new: bool;

  for idx in 0..hand.cards.len() {
    new = true;

    for i in 0..cards.len() {
      if hand.cards[idx] == cards[i].0 {
        new = false;
        cards[i].1 += 1;
        break;
      }
    }

    if new {
      cards.push((hand.cards[idx], 1));
    }
  }

  let mut hand_type = 0;
  let mut j_card = -1;

  for card in &cards {
    if card.0 == 'J' {
      j_card = card.1;
      continue;
    }

    if card.1 == 2 {
      if hand_type == 0 {
        // first pair
        hand_type = 1;
      } else if hand_type == 1 {
        // second pair - two pairs
        hand_type = 2;
      } else if hand_type == 3 {
        // pair with 3 of kind - full house
        hand_type = 4;
      } else {
        panic!();
      }
    } else if card.1 == 3 {
      if hand_type == 0 {
        // first 3 of kind
        hand_type = 3
      } else if hand_type == 1 {
        // 3 of kind with a pair - full house
        hand_type = 4;
      } else {
        panic!();
      }
    } else if card.1 == 4 {
      if hand_type == 0 {
        // first 4 of kind
        hand_type = 5;
      } else {
        panic!();
      }
    } else if card.1 == 5 {
      if hand_type == 0 {
        // first 5 of kind
        hand_type = 6;
      } else {
        panic!();
      }
    }
  }

  // include jokers
  if j_card > 0 {
    if hand_type == 1 {
      // Just a pair
      if j_card == 1 {
        // three of a kind
        hand_type = 3;
      } else if j_card == 2 {
        // four of a kind
        hand_type = 5;
      } else if j_card == 3 {
        // five of a kind
        hand_type = 6;
      } else {
        panic!();
      }
    } else if hand_type == 2 {
      // two pairs
      if j_card == 1 {
        // full house
        hand_type = 4;
      } else {
        panic!();
      }
    } else if hand_type == 3 {
      // three of a kind
      if j_card == 1 {
        // four of a kind
        hand_type = 5;
      } else if j_card == 2 {
        // five of a kind
        hand_type = 6;
      }
    } else if hand_type == 5 {
      // four of a kind
      if j_card == 1 {
        // five of a kind
        hand_type = 6;
      } else {
        panic!();
      }
    } else if hand_type == 0 {
      if j_card == 1 {
        // a pair
        hand_type = 1;
      } else if j_card == 2 {
        // three of a kind
        hand_type = 3;
      } else if j_card == 3 {
        // four of a kind
        hand_type = 5;
      } else if j_card == 4 {
        // five of a kind
        hand_type = 6;
      } else if j_card == 5 {
        // five of a kind
        hand_type = 6;
      } else {
        panic!()
      }
    } else {
      panic!();
    }
  }

  hand.hand_type = hand_type;
}

fn add_with_sort(hand: Hand, hands: &mut Vec<Hand>, order: &[char]) {
  for idx in 0..hands.len() {
    if &hand.hand_type > &hands[idx].hand_type {
      continue;
    }

    if &hand.hand_type < &hands[idx].hand_type {
      hands.insert(idx, hand);
      return;
    }

    if &hand.hand_type == &hands[idx].hand_type {
      // Hands have the same hand type - need to check the card order
      let mut card_idx_1: usize;
      let mut card_idx_2: usize;
      for i in 0..hand.cards.len() {
        card_idx_1 = order
          .iter()
          .position(|&item| &item == &hand.cards[i])
          .unwrap();
        card_idx_2 = order
          .iter()
          .position(|&item| &item == &hands[idx].cards[i])
          .unwrap();
        if card_idx_1 == card_idx_2 {
          continue;
        }

        if card_idx_1 > card_idx_2 {
          hands.insert(idx, hand); // Next card is stronger - insert before it
          return;
        } else {
          break;
        }
      }
    }
  }

  hands.push(hand);
}
