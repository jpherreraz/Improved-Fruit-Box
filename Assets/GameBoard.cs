using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    [SerializeField]
    private Apple apple;
    [SerializeField]
    private Selector selector;
    private int[,] board = {
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0},
        {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0}
    };
    private Apple[,] appleBoard = new Apple[10,17];
    private int rows = 10;
    private int cols = 17;


    // Start is called before the first frame update
    void Start()
    {
        GenerateGroups();
        GenerateApples();
        //testthing();
    }

    // Update is called once per frame
    void Update()
    {   
        int total = 0;
        if (selector.hasFinishedDragging()) {
            float startX = adjustBounds(selector.getStartMousePos().x, true);
            float endX = adjustBounds(selector.getCurrentMousePos().x, true);
            float startY = adjustBounds(selector.getStartMousePos().y, false);
            float endY = adjustBounds(selector.getCurrentMousePos().y, false);
            float signX = Mathf.Sign(endX - startX);
            float signY = Mathf.Sign(endY - startY);
            for (float i = (int) (startX + 0.5 + signX/2); i * signX < endX * signX; i += signX) {
                for (float j = (int) (startY + 0.5 + signY/2); j * signY < endY * signY; j += signY) {
                    if (appleBoard[(int) j, (int) i] != null) {
                        int value = appleBoard[(int) j, (int) i].GetValue();
                        total += value;
                    }
                }
            }
            Debug.Log(total);
            if (total == 10) {
                for (float i = (int) (startX + 0.5 + signX/2); i * signX < endX * signX; i += signX) {
                    for (float j = (int) (startY + 0.5 + signY/2); j * signY < endY * signY; j += signY) {
                        if (appleBoard[(int) j, (int) i] != null) {
                            Destroy(appleBoard[(int) j, (int) i].gameObject);
                            Debug.Log("Destroyed (" + i + ", " + j + ")");
                            appleBoard[(int) j, (int) i] = null;
                        }
                    }
                }
            }
        }
    }
    
    private void GenerateGroups() {
        while (!IsDone()) {
            if (getNumZeroes() == 2) {
                GenerateGroup(2);
            }
            else if (getNumZeroes() == 3) {
                GenerateGroup(3);
            }
            else if (getNumZeroes() == 4) {
                if (Random.Range(0,2) == 0) {
                    GenerateGroup(2);
                    GenerateGroup(2);
                } else {
                    GenerateGroup(4);
                }
            } else if (getNumZeroes() == 5) {
                if (Random.Range(0,2) == 0) {
                    GenerateGroup(2);
                    GenerateGroup(3);
                } else {
                    GenerateGroup(5);
                }
            } else if (getNumZeroes() == 6) {
                if (Random.Range(0,3) == 0) {
                    GenerateGroup(2);
                } else {
                    GenerateGroup(3);
                    GenerateGroup(3);
                }
                
            } else {
                int rand2 = Random.Range(0,101);
                if (0 < rand2 && rand2 <= 55) {
                    GenerateGroup(2);
                } else if (55 < rand2 && rand2 <= 80) {
                    GenerateGroup(3);
                } else if (80 < rand2 && rand2 <= 97) {
                    GenerateGroup(4);
                } else {
                    GenerateGroup(5);
                }
            }
        }
    }
    private void GenerateGroup(int size) {
        int[] numbers = GenerateNumbers(size);
        int[,] coordPath = new int[size, 2];
        int[] coords = GenerateCoords();
        coordPath[0,0] = coords[0];
        coordPath[0,1] = coords[1];
        board[coords[0], coords[1]] = numbers[0];
        for (int i = 1; i < size; i++) {
            FindNextCoords(coords);
            coordPath[i, 0] = coords[0];
            coordPath[i, 1] = coords[1];
            board[coords[0], coords[1]] = numbers[i];
        }

    }
    private void GenerateApple(int x, int y) {
        apple.transform.position = new Vector3(y, x, 0);
        appleBoard[x, y] = Instantiate(apple);
        appleBoard[x, y].SetValue(board[x, y]);
    }

    private void GenerateApples() {
        for (int x = 0; x < rows; x++) {
            for (int y = 0; y < cols; y++) {
                GenerateApple(x, y);
            }
        }
    }
    private bool IsDone() {
        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                if (board[i,j] == 0) {
                    return false;
                
                }
            }
        }
        return true;
    }
    private int[] GenerateNumbers(int size) {
        int total = 0;
        int[] result = new int[size];
        for (int i = 0; i < size; i++) {
            result[i] = Random.Range(1,10);
            total += result[i];
        }
        if (total != 10) {
            return GenerateNumbers(size);
        }
        return result;
    }
    private int[] GenerateCoords() {
        int[] result = new int[2];
        result[0] = Random.Range(0, rows);
        result[1] = Random.Range(0, cols);
        if (board[result[0], result[1]] != 0) {
            return GenerateCoords();
        }
        return result;
    }
    private void FindNextCoords(int[] currentCoords) {
        // 0 = up, 1 = down, 2 = left, 3 = right
        int direction = Random.Range(0,4);
        switch (direction) {
            case 0:
                while (board[currentCoords[0], currentCoords[1]] != 0) { 
                    if (currentCoords[0] == 0 && currentCoords[1] == 16) {
                        currentCoords[0] = 9;
                        currentCoords[1] = 0;
                    }else if (currentCoords[0] == 0) {
                        currentCoords[0] = 9;
                        currentCoords[1]++;
                    }
                    else {
                        currentCoords[0]--;
                    }
                }
                break;
            case 1:
                while (board[currentCoords[0], currentCoords[1]] != 0) { 
                    if (currentCoords[0] == 9 && currentCoords[1] == 16) {
                        currentCoords[0] = 0;
                        currentCoords[1] = 0;
                    }else if (currentCoords[0] == 9) {
                        currentCoords[0] = 0;
                        currentCoords[1]++;
                    }
                    else {
                        currentCoords[0]++;
                    }
                }
                break;
            case 2:
                while (board[currentCoords[0], currentCoords[1]] != 0) { 
                    if (currentCoords[0] == 9 && currentCoords[1] == 0) {
                        currentCoords[0] = 0;
                        currentCoords[1] = 16;
                    }else if (currentCoords[1] == 0) {
                        currentCoords[1] = 16;
                        currentCoords[0]++;
                    }
                    else {
                        currentCoords[1]--;
                    }
                }
                break;
            case 3:
                while (board[currentCoords[0], currentCoords[1]] != 0) { 
                    if (currentCoords[0] == 9 && currentCoords[1] == 16) {
                        currentCoords[0] = 0;
                        currentCoords[1] = 0;
                    }else if (currentCoords[1] == 16) {
                        currentCoords[1] = 0;
                        currentCoords[0]++;
                    }
                    else {
                        currentCoords[1]++;
                    }
                }
                break;
            default:
                break;
        }
    }

    private int getNumZeroes() {
        int zeroes = 0;
        for (int i = 0; i < rows; i++) {
            for(int j = 0; j < cols; j++) {
                if (board[i, j] == 0) {
                    zeroes++;
                }
            }
        }
        return zeroes;
    }
    private float adjustBounds(float num, bool isX) {
        if (num <= -1) {
            return -0.5f;
        } else if (isX && num >= cols) {
            return cols - 0.5f;
        } else if (!isX && num >= rows) {
            return rows - 0.5f;
        } else {
            return num;
        }
    }
}
