import { useEffect, useState } from "react";
import { branchOfficeService } from "../api/branchOfficeService";
import { departmentService } from "../api/departmentService";
import { employeeService } from "../api/employeeService";
import { taskService } from "../api/taskService";
import { Layout } from "../components/Layout";
import { useAuth } from "../context/AuthContext";
import {
	TaskPriority,
	TaskStatus,
	type BranchOffice,
	type Department,
	type Employee,
	type OfficeTask,
} from "../types";

export const Tasks = () => {
	const [tasks, setTasks] = useState<OfficeTask[]>([]);
	const [branchOffices, setBranchOffices] = useState<BranchOffice[]>([]);
	const [departments, setDepartments] = useState<Department[]>([]);
	const [employees, setEmployees] = useState<Employee[]>([]);
	const [loading, setLoading] = useState(true);
	const [showModal, setShowModal] = useState(false);
	const [editingTask, setEditingTask] = useState<OfficeTask | null>(null);
	const { isAdmin, user } = useAuth();

	const [formData, setFormData] = useState({
		title: "",
		description: "",
		priority: TaskPriority.Medium,
		status: TaskStatus.Pending,
		branchOfficeId: "",
		departmentId: "",
		assignedEmployeeId: "",
		dueDate: "",
	});

	useEffect(() => {
		loadData();
	}, []);

	const loadData = async () => {
		try {
			const [tasksData, branchOfficesData, departmentsData, employeesData] = await Promise.all([
				taskService.getAll(),
				branchOfficeService.getAll(),
				departmentService.getAll(),
				employeeService.getAll(),
			]);
			setTasks(tasksData);
			setBranchOffices(branchOfficesData);
			setDepartments(departmentsData);
			setEmployees(employeesData);
		} catch (error) {
			console.error("Ошибка загрузки данных:", error);
		} finally {
			setLoading(false);
		}
	};

	const handleSubmit = async (e: React.FormEvent) => {
		e.preventDefault();
		try {
			if (editingTask) {
				await taskService.update(editingTask.id, {
					title: formData.title,
					description: formData.description,
					status: formData.status,
					priority: formData.priority,
					branchOfficeId: formData.branchOfficeId || undefined,
					departmentId: formData.departmentId || undefined,
					assignedEmployeeId: formData.assignedEmployeeId || undefined,
					dueDate: formData.dueDate || undefined,
				});
			} else {
				await taskService.create({
					title: formData.title,
					description: formData.description,
					priority: formData.priority,
					branchOfficeId: formData.branchOfficeId || undefined,
					departmentId: formData.departmentId || undefined,
					assignedEmployeeId: formData.assignedEmployeeId || undefined,
					dueDate: formData.dueDate || undefined,
				});
			}
			setShowModal(false);
			setEditingTask(null);
			resetForm();
			loadData();
		} catch (error) {
			console.error("Ошибка при сохранении задачи:", error);
		}
	};

	const handleEdit = (task: OfficeTask) => {
		setEditingTask(task);
		setFormData({
			title: task.title,
			description: task.description,
			priority: task.priority,
			status: task.status,
			branchOfficeId: task.branchOfficeId || "",
			departmentId: task.departmentId || "",
			assignedEmployeeId: task.assignedEmployeeId || "",
			dueDate: task.dueDate ? task.dueDate.split("T")[0] : "",
		});
		setShowModal(true);
	};

	const handleDelete = async (id: string) => {
		if (!confirm("Вы уверены, что хотите удалить эту задачу?")) return;
		try {
			await taskService.delete(id);
			loadData();
		} catch (error) {
			console.error("Ошибка при удалении задачи:", error);
		}
	};

	const resetForm = () => {
		setFormData({
			title: "",
			description: "",
			priority: TaskPriority.Medium,
			status: TaskStatus.Pending,
			branchOfficeId: "",
			departmentId: "",
			assignedEmployeeId: "",
			dueDate: "",
		});
	};

	const getStatusColor = (status: TaskStatus) => {
		switch (status) {
			case TaskStatus.Pending:
				return "bg-yellow-100 text-yellow-800";
			case TaskStatus.InProgress:
				return "bg-blue-100 text-blue-800";
			case TaskStatus.Completed:
				return "bg-green-100 text-green-800";
			case TaskStatus.Cancelled:
				return "bg-red-100 text-red-800";
			default:
				return "bg-gray-100 text-gray-800";
		}
	};

	const getStatusText = (status: TaskStatus) => {
		switch (status) {
			case TaskStatus.Pending:
				return "Ожидает";
			case TaskStatus.InProgress:
				return "В работе";
			case TaskStatus.Completed:
				return "Завершена";
			case TaskStatus.Cancelled:
				return "Отменена";
			default:
				return "Неизвестно";
		}
	};

	const getPriorityText = (priority: TaskPriority) => {
		switch (priority) {
			case TaskPriority.Low:
				return "Низкий";
			case TaskPriority.Medium:
				return "Средний";
			case TaskPriority.High:
				return "Высокий";
			case TaskPriority.Critical:
				return "Критический";
			default:
				return "Неизвестно";
		}
	};

	if (loading) {
		return (
			<Layout>
				<div className="text-center py-12">Загрузка...</div>
			</Layout>
		);
	}

	return (
		<Layout>
			<div className="px-4 py-6 sm:px-0">
				<div className="flex justify-between items-center mb-6">
					<h1 className="text-3xl font-bold text-gray-900">Задачи</h1>
					{isAdmin && (
						<button
							onClick={() => {
								setEditingTask(null);
								resetForm();
								setShowModal(true);
							}}
							className="bg-indigo-600 hover:bg-indigo-700 text-white px-4 py-2 rounded-md text-sm font-medium"
						>
							Добавить задачу
						</button>
					)}
				</div>

				<div className="bg-white shadow overflow-hidden sm:rounded-md">
					<ul className="divide-y divide-gray-200">
						{tasks.map((task) => (
							<li key={task.id}>
								<div className="px-4 py-4 sm:px-6">
									<div className="flex items-center justify-between">
										<div className="flex-1">
											<div className="flex items-center">
												<div className="text-sm font-medium text-gray-900">
													{task.title}
												</div>
												<span
													className={`ml-2 inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${getStatusColor(
														task.status
													)}`}
												>
													{getStatusText(task.status)}
												</span>
												<span className="ml-2 text-xs text-gray-500">
													Приоритет: {getPriorityText(task.priority)}
												</span>
											</div>
											<div className="mt-1 text-sm text-gray-500">
												{task.description}
											</div>
											<div className="mt-1 text-xs text-gray-400">
												{task.branchOffice && `Офис: ${task.branchOffice.name} • `}
												{task.department && `Отдел: ${task.department.name} • `}
												{task.assignedEmployee &&
													`Исполнитель: ${task.assignedEmployee.firstName} ${task.assignedEmployee.lastName}`}
												{task.dueDate &&
													` • Срок: ${new Date(task.dueDate).toLocaleDateString(
														"ru-RU"
													)}`}
											</div>
										</div>
										<div className="flex space-x-2">
											<button
												onClick={() => handleEdit(task)}
												className="text-indigo-600 hover:text-indigo-900 text-sm font-medium"
											>
												Редактировать
											</button>
											{isAdmin && (
												<button
													onClick={() => handleDelete(task.id)}
													className="text-red-600 hover:text-red-900 text-sm font-medium"
												>
													Удалить
												</button>
											)}
										</div>
									</div>
								</div>
							</li>
						))}
					</ul>
				</div>

				{showModal && (
					<div className="fixed z-10 inset-0 overflow-y-auto">
						<div className="flex items-end justify-center min-h-screen pt-4 px-4 pb-20 text-center sm:block sm:p-0">
							<div
								className="fixed inset-0 bg-gray-500 bg-opacity-75 transition-opacity"
								onClick={() => setShowModal(false)}
							></div>
							<div className="inline-block align-bottom bg-white rounded-lg text-left overflow-hidden shadow-xl transform transition-all sm:my-8 sm:align-middle sm:max-w-lg sm:w-full">
								<form
									onSubmit={handleSubmit}
									className="bg-white px-4 pt-5 pb-4 sm:p-6 sm:pb-4"
								>
									<h3 className="text-lg leading-6 font-medium text-gray-900 mb-4">
										{editingTask ? "Редактировать задачу" : "Добавить задачу"}
									</h3>
									<div className="space-y-4">
										<div>
											<label className="block text-sm font-medium text-gray-700">
												Название
											</label>
											<input
												type="text"
												required
												className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
												value={formData.title}
												onChange={(e) =>
													setFormData({ ...formData, title: e.target.value })
												}
											/>
										</div>
										<div>
											<label className="block text-sm font-medium text-gray-700">
												Описание
											</label>
											<textarea
												required
												rows={3}
												className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
												value={formData.description}
												onChange={(e) =>
													setFormData({
														...formData,
														description: e.target.value,
													})
												}
											/>
										</div>
										<div>
											<label className="block text-sm font-medium text-gray-700">
												Приоритет
											</label>
											<select
												required
												className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
												value={formData.priority}
												onChange={(e) =>
													setFormData({
														...formData,
														priority: Number(e.target.value) as TaskPriority,
													})
												}
											>
												<option value={TaskPriority.Low}>Низкий</option>
												<option value={TaskPriority.Medium}>Средний</option>
												<option value={TaskPriority.High}>Высокий</option>
												<option value={TaskPriority.Critical}>
													Критический
												</option>
											</select>
										</div>
										{editingTask && (
											<div>
												<label className="block text-sm font-medium text-gray-700">
													Статус
												</label>
												<select
													required
													className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
													value={formData.status}
													onChange={(e) =>
														setFormData({
															...formData,
															status: Number(e.target.value) as TaskStatus,
														})
													}
												>
													<option value={TaskStatus.Pending}>Ожидает</option>
													<option value={TaskStatus.InProgress}>
														В работе
													</option>
													<option value={TaskStatus.Completed}>
														Завершена
													</option>
													<option value={TaskStatus.Cancelled}>Отменена</option>
												</select>
											</div>
										)}
										<div>
											<label className="block text-sm font-medium text-gray-700">
												Офис
											</label>
											<select
												className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
												value={formData.branchOfficeId}
												onChange={(e) =>
													setFormData({
														...formData,
														branchOfficeId: e.target.value,
													})
												}
											>
												<option value="">Не выбрано</option>
												{branchOffices.map((bo) => (
													<option key={bo.id} value={bo.id}>
														{bo.name}
													</option>
												))}
											</select>
										</div>
										<div>
											<label className="block text-sm font-medium text-gray-700">
												Отдел
											</label>
											<select
												className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
												value={formData.departmentId}
												onChange={(e) =>
													setFormData({
														...formData,
														departmentId: e.target.value,
													})
												}
											>
												<option value="">Не выбрано</option>
												{departments.map((dept) => (
													<option key={dept.id} value={dept.id}>
														{dept.name}
													</option>
												))}
											</select>
										</div>
										<div>
											<label className="block text-sm font-medium text-gray-700">
												Исполнитель
											</label>
											<select
												className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
												value={formData.assignedEmployeeId}
												onChange={(e) =>
													setFormData({
														...formData,
														assignedEmployeeId: e.target.value,
													})
												}
											>
												<option value="">Не выбрано</option>
												{employees.map((emp) => (
													<option key={emp.id} value={emp.id}>
														{emp.firstName} {emp.lastName}
													</option>
												))}
											</select>
										</div>
										<div>
											<label className="block text-sm font-medium text-gray-700">
												Срок выполнения
											</label>
											<input
												type="date"
												className="mt-1 block w-full border border-gray-300 rounded-md shadow-sm py-2 px-3 focus:outline-none focus:ring-indigo-500 focus:border-indigo-500"
												value={formData.dueDate}
												onChange={(e) =>
													setFormData({ ...formData, dueDate: e.target.value })
												}
											/>
										</div>
									</div>
									<div className="mt-5 sm:mt-6 sm:grid sm:grid-cols-2 sm:gap-3 sm:grid-flow-row-dense">
										<button
											type="submit"
											className="w-full inline-flex justify-center rounded-md border border-transparent shadow-sm px-4 py-2 bg-indigo-600 text-base font-medium text-white hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 sm:col-start-2 sm:text-sm"
										>
											{editingTask ? "Обновить" : "Создать"}
										</button>
										<button
											type="button"
											onClick={() => {
												setShowModal(false);
												setEditingTask(null);
												resetForm();
											}}
											className="mt-3 w-full inline-flex justify-center rounded-md border border-gray-300 shadow-sm px-4 py-2 bg-white text-base font-medium text-gray-700 hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500 sm:mt-0 sm:col-start-1 sm:text-sm"
										>
											Отмена
										</button>
									</div>
								</form>
							</div>
						</div>
					</div>
				)}
			</div>
		</Layout>
	);
};

