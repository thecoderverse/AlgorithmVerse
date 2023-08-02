/*
Usage: g++ -std=c++20 -Ofast .\solution-1.cpp -o solution-1.exe && .\solution-1.exe
After that write the initial board to the terminal.
*/

#include <algorithm>
#include <array>
#include <bitset>
#include <iostream>
#include <memory>
#include <numeric>
#include <queue>
#include <unordered_set>

const std::array<std::bitset<4>, 6> possible_directions = {0b0110, 0b1110, 0b1100, 0b0011, 0b1011, 0b1001}; // left, bottom, right, top
const std::array<int8_t, 4> direction_offset = {-3, 1, 3, -1}; // top, right, bottom, left
const uint32_t winning_hash = 0b001010011100101000; // { 1, 2, 3, 4, 5, 0 } (3 bit groups)

class Board {
private:
    std::array<uint8_t, 6> tiles;
    uint8_t empty_location;
    uint8_t current_direction = 0;
    uint32_t depth = 0;

public:
    Board(std::array<uint8_t, 6> tiles): tiles{tiles} {
        empty_location = std::distance(
            tiles.begin(), 
            std::find(tiles.begin(), tiles.end(), 0)
        );
    }

    std::shared_ptr<Board> next_move() {
        if (current_direction > 3) return nullptr;

        auto directions = possible_directions[empty_location];

        if (!directions.test(current_direction)) {
            current_direction++;
            return this->next_move();
        }

        uint8_t tile = empty_location + direction_offset[current_direction++];

        return this->swap(
            tile,
            empty_location
        );
    }

    uint32_t get_hash() const {
        return std::accumulate(
            tiles.begin(),
            tiles.end(),
            (uint32_t)0,
            [](uint32_t hash, uint8_t tile) { return (hash << 3) | tile; }
        );
    }

    uint32_t get_depth() const { 
        return depth; 
    }

private:
    Board(std::array<uint8_t, 6> tiles, uint32_t depth): Board(tiles) {
        this->depth = depth;
    }

    std::shared_ptr<Board> swap(uint8_t tile, uint8_t empty) {
        std::array<uint8_t, 6> new_state = tiles;
        std::swap(new_state[tile], new_state[empty]);
        return std::make_shared<Board>(Board(new_state, depth + 1));
    }
};

class Solution {
private:
    std::unordered_set<uint32_t> previous_boards;
    std::queue<std::shared_ptr<Board>> bfs_queue;

public:
    Solution() { }

    int64_t solve(std::shared_ptr<Board> board) {
        if (board->get_hash() == winning_hash) return 0;

        bfs_queue = std::queue<std::shared_ptr<Board>>({board});

        while (!bfs_queue.empty()) {
            auto front = bfs_queue.front();

            auto next_board = front->next_move();

            if (next_board == nullptr) {
                bfs_queue.pop();
                continue;
            }

            auto hash = next_board->get_hash(); 

            if (hash == winning_hash) return next_board->get_depth();

            if (previous_boards.contains(hash)) continue;

            previous_boards.insert(hash);
            bfs_queue.push(next_board);
        }

        return -1;
    }
};

std::array<uint8_t, 6> parse(std::string str_board) {
    uint8_t index = 0;
    return std::accumulate(
        str_board.begin(),
        str_board.end(),
        std::array<uint8_t, 6>(),
        [index](std::array<uint8_t, 6> tiles, char ch) mutable {
            if (isdigit(ch)) tiles[index++] = ch - '0';
            return tiles;
        }
    );
}

void solve(std::string str_board) {
    auto tiles = parse(str_board);
    auto board = std::make_shared<Board>(Board(tiles));
    auto solution = Solution();
    std::cout << solution.solve(board) << "\n";
}

int main(void) {
    std::string board;
    std::cin >> board;
    solve(board);
}