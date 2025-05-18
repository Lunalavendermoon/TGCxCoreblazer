public class BlockType {
    public string name;

    public string displayName;

    public string foodGroup;

    public int size;

    public bool[][] shape;

    public BlockType(string _name, string _foodGroup, bool[][] _shape) {
        name = _name;
        displayName = _name;
        foodGroup = _foodGroup;

        shape = _shape;

        size = 0;
        
        for (int i = 0; i < shape.Length; ++i) {
            for (int j = 0; j < shape[i].Length; ++j) {
                if (shape[i][j]) {
                    ++size;
                }
            }
        }
    }

    public BlockType(string _name, string display, string _foodGroup, bool[][] _shape) {
        name = _name;
        displayName = display;
        foodGroup = _foodGroup;

        shape = _shape;

        size = 0;
        
        for (int i = 0; i < shape.Length; ++i) {
            for (int j = 0; j < shape[i].Length; ++j) {
                if (shape[i][j]) {
                    ++size;
                }
            }
        }
    }

    // FRUITS VEGETABLES

    public static BlockType apple() {
        return new BlockType("apple", "veg", new bool[][] {
            new bool[] {true, true},
            new bool[] {true, true}
        });
    }

    public static BlockType mushroom() {
        return new BlockType("mushroom", "veg", new bool[][] {
            new bool[] {true, true, true},
            new bool[] {false, true, false}
        });
    }

    public static BlockType banana() {
        return new BlockType("banana", "veg", new bool[][] {
            new bool[] {true, false, false},
            new bool[] {true, true, true}
        });
    }

    public static BlockType lettuce() {
        return new BlockType("lettuce", "veg", new bool[][] {
            new bool[] {false, false, true},
            new bool[] {false, true, true},
            new bool[] {true, true, true}
        });
    }

    public static BlockType tomato() {
        return new BlockType("tomato", "veg", new bool[][] {
            new bool[] {false, true, false},
            new bool[] {true, true, false},
            new bool[] {true, true, true}
        });
    }

    public static BlockType cucumber() {
        return new BlockType("cucumber", "veg", new bool[][] {
            new bool[] {true, true, true}
        });
    }

    public static BlockType cabbage() {
        return new BlockType("cabbage", "veg", new bool[][] {
            new bool[] {true, true, true},
            new bool[] {true, true, true}
        });
    }

    public static BlockType broccoli() {
        return new BlockType("broccoli", "veg", new bool[][] {
            new bool[] {true, true, true},
            new bool[] {false, true, false},
            new bool[] {false, true, false}
        });
    }

    public static BlockType eggplant() {
        return new BlockType("eggplant", "veg", new bool[][] {
            new bool[] {true, false},
            new bool[] {true, true}
        });
    }

    public static BlockType carrot() {
        return new BlockType("carrot", "veg", new bool[][] {
            new bool[] {true},
            new bool[] {true}
        });
    }

    // CARB

    public static BlockType noodle() {
        return new BlockType("noodle", "carb", new bool[][] {
            new bool[] {true, true, true},
            new bool[] {true, true, true}
        });
    }

    public static BlockType rice() {
        return new BlockType("rice", "carb", new bool[][] {
            new bool[] {true, false, true},
            new bool[] {true, true, true}
        });
    }

    public static BlockType corn() {
        return new BlockType("corn", "carb", new bool[][] {
            new bool[] {true, true, true},
        });
    }

    public static BlockType bread() {
        return new BlockType("bread", "carb", new bool[][] {
            new bool[] {true, false, false},
            new bool[] {true, true, true}
        });
    }

    public static BlockType bread2() {
        return new BlockType("bread2", "bread", "carb", new bool[][] {
            new bool[] {true, true, false, false},
            new bool[] {false, true, true, true}
        });
    }

    public static BlockType bread3() {
        return new BlockType("bread3", "bread loaf", "carb", new bool[][] {
            new bool[] {true, true, false},
            new bool[] {true, true, true}
        });
    }

    public static BlockType bread4() {
        return new BlockType("bread4", "baguette", "carb", new bool[][] {
            new bool[] {true, true, true, true}
        });
    }

    public static BlockType oats() {
        return new BlockType("oats", "carb", new bool[][] {
            new bool[] {true, false, false},
            new bool[] {true, true, false},
            new bool[] {false, true, true}
        });
    }

    // PROTEINS

    public static BlockType chickenLeg() {
        return new BlockType("chicken leg", "protein", new bool[][] {
            new bool[] {true, false},
            new bool[] {true, true}
        });
    }

    public static BlockType tofu() {
        return new BlockType("tofu", "protein", new bool[][] {
            new bool[] {false, true, false},
            new bool[] {true, true, false},
            new bool[] {true, true, true}
        });
    }

    public static BlockType ham() {
        return new BlockType("ham", "protein", new bool[][] {
            new bool[] {true, true, true},
            new bool[] {false, true, true}
        });
    }

    public static BlockType egg() {
        return new BlockType("egg", "protein", new bool[][] {
            new bool[] {false, false, true},
            new bool[] {false, false, true},
            new bool[] {true, true, true}
        });
    }

    public static BlockType fish() {
        return new BlockType("fish", "protein", new bool[][] {
            new bool[] {true, true, false},
            new bool[] {false, true, true}
        });
    }

    public static BlockType fishSlice() {
        return new BlockType("fish slice", "protein", new bool[][] {
            new bool[] {false, true, true},
            new bool[] {true, true, false},
            new bool[] {true, false, false}
        });
    }

    public static BlockType bean() {
        return new BlockType("beans", "protein", new bool[][] {
            new bool[] {true, true},
            new bool[] {true, true}
        });
    }

    public static BlockType bacon() {
        return new BlockType("bacon", "protein", new bool[][] {
            new bool[] {true},
            new bool[] {true}
        });
    }

    public static BlockType cheese() {
        return new BlockType("cheese", "protein", new bool[][] {
            new bool[] {true, true, true},
            new bool[] {true, true, true}
        });
    }
}