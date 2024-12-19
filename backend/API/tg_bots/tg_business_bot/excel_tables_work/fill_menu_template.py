import openpyxl
from Model.Menu import Menu
from Model.Dish import Dish
from Model.Drink import Drink


def fill_menu_template(template_path, output_file_path, menu: Menu):
    wb = openpyxl.load_workbook(template_path)
    dishes_sheet = wb["Блюда"]
    drinks_sheet = wb["Напитки"]

    drink_row_num = 3
    dish_row_num = 3

    for category in menu.categories:
        for item in category.menu_items:
            sheet, weight_or_volume, is_drink = _get_sheet_and_weight_or_volume(
                item, dishes_sheet, drinks_sheet)
            if sheet is None:
                continue

            if is_drink:
                _fill_row(sheet, drink_row_num, category, item,
                          weight_or_volume)
                drink_row_num += 1
            else:
                _fill_row(sheet, dish_row_num, category, item,
                          weight_or_volume)
                dish_row_num += 1

    wb.save(output_file_path)


def _get_sheet_and_weight_or_volume(item, dishes_sheet, drinks_sheet):
    if isinstance(item, Drink):
        return drinks_sheet, item.volume, True
    elif isinstance(item, Dish):
        return dishes_sheet, item.weight, False
    return None, None, False


def _fill_row(sheet, row_num, category, item, weight_or_volume):
    data = [
        category.name,
        item.name,
        item.description,
        item.price,
        weight_or_volume,
        item.nutrients_of_100_grams.kilocalories,
        item.nutrients_of_100_grams.proteins,
        item.nutrients_of_100_grams.fats,
        item.nutrients_of_100_grams.carbohydrates,
        item.cooking_time_minutes
    ]

    for col_num, value in enumerate(data, start=2):
        if value is not None:
            sheet.cell(row=row_num, column=col_num, value=value)

    # Вставлять картинку в ячейку эксель
    '''if item.image_path is not None:
        img = Image(item.image_path)
        img.anchor = f'K{row_num}'
        sheet.add_image(img)'''
